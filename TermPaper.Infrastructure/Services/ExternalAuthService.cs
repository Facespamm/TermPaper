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
        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false);

        if (signInResult.Succeeded)
        {
            return Result.Success();
        }
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing != null)
        {
            var logins = await _userManager.GetLoginsAsync(existing);
            var alreadyLinked = logins.Any(l =>
                l.LoginProvider == info.LoginProvider &&
                l.ProviderKey == info.ProviderKey);

            if (alreadyLinked && !existing.EmailConfirmed)
            {
                await SendConfirmationEmailAsync(existing, email);
                return Result.Failure(ErrorCode.EmailNotConfirmed);
            }
            return Result.Failure(ErrorCode.AlreadyExists);
        }

        var isTrusted = TrustedProviders.Contains(info.LoginProvider);

        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = isTrusted
        };

        var createResult = await _userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Candidate");
        if (!roleResult.Succeeded)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
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