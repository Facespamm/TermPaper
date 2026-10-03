using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using TermPaper.Application.Common;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;
using TermPaper.Infrastructure.Identity;

namespace TermPaper.Infrastructure.Services;

public class ExternalAuthService : IExternalAuthService
{
    private static readonly string[] TrustedProviders = { "Google" };

    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ISenderEmailService _emailSender;
    private readonly string _baseUrl;

    public ExternalAuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ISenderEmailService emailSender,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _baseUrl = configuration["AppSettings:BaseUrl"] ?? "http://localhost:5000";
    }

    public async Task<Result> ExternalLoginAsync()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        if (await TrySignInExistingAsync(info))
        {
            return Result.Success();
        }
        var email = GetEmail(info);
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
        {
            return await HandleExistingUserAsync(existing, info, email);
        }
        return await RegisterNewUserAsync(info, email);
    }

    private async Task<bool> TrySignInExistingAsync(ExternalLoginInfo info)
    {
        var result = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false);
        return result.Succeeded;
    }

    private static string? GetEmail(ExternalLoginInfo info)
    {
        return info.Principal.FindFirstValue(ClaimTypes.Email);
    }

    private static bool IsTrustedProvider(string provider)
    {
        return TrustedProviders.Contains(provider);
    }

    private async Task<bool> IsLoginLinkedAsync(AppUser user, ExternalLoginInfo info)
    {
        var logins = await _userManager.GetLoginsAsync(user);
        return logins.Any(l =>
            l.LoginProvider == info.LoginProvider &&
            l.ProviderKey == info.ProviderKey);
    }

    private async Task<Result> HandleExistingUserAsync(
        AppUser existing,
        ExternalLoginInfo info,
        string email)
    {
        var alreadyLinked = await IsLoginLinkedAsync(existing, info);
        if (alreadyLinked && !existing.EmailConfirmed)
        {
            await SendConfirmationEmailAsync(existing, email);
            return Result.Failure(ErrorCode.EmailNotConfirmed);
        }
        if (!alreadyLinked && CanAutoLink(existing, info))
        {
            var addLoginResult = await _userManager.AddLoginAsync(existing, info);
            if (!addLoginResult.Succeeded)
            {
                return Result.Failure(ErrorCode.ValidationFailed);
            }
            await _signInManager.SignInAsync(existing, isPersistent: false);
            return Result.Success();
        }
        return Result.Failure(ErrorCode.AlreadyExists);
    }

    private static bool CanAutoLink(AppUser existing, ExternalLoginInfo info)
    {
        return IsTrustedProvider(info.LoginProvider) && existing.EmailConfirmed;
    }
    private async Task<Result> RegisterNewUserAsync(ExternalLoginInfo info, string email)
    {
        var isTrusted = IsTrustedProvider(info.LoginProvider);

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = isTrusted
        };
        if (!await CreateUserWithLoginAsync(user, info))
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        if (!isTrusted)
        {
            await SendConfirmationEmailAsync(user, email);
            return Result.Failure(ErrorCode.EmailNotConfirmed);
        }
        await _signInManager.SignInAsync(user, isPersistent: false);
        return Result.Success();
    }

    private async Task<bool> CreateUserWithLoginAsync(AppUser user, ExternalLoginInfo info)
    {
        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            return false;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Candidate");
        if (!roleResult.Succeeded)
        {
            return false;
        }

        var loginResult = await _userManager.AddLoginAsync(user, info);
        return loginResult.Succeeded;
    }

    private async Task SendConfirmationEmailAsync(AppUser user, string email)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmLink = QueryHelpers.AddQueryString(
            $"{_baseUrl}/ConfirmEmailPage",
            new Dictionary<string, string?>
            {
                ["email"] = email,
                ["token"] = token
            });
        await _emailSender.SendEmailAsync(email, confirmLink);
    }
}