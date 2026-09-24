using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TermPaper.Application.Common;
using TermPaper.Enum;
using TermPaper.Interface;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Services;

public class ExternalAuthService : IExternalAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public ExternalAuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
            var link = await _userManager.AddLoginAsync(existing, info);
            if (!link.Succeeded) return Result.Failure(ErrorCode.ValidationFailed);
            await _signInManager.SignInAsync(existing, isPersistent: false);
            return Result.Success();
        }
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
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

        await _signInManager.SignInAsync(user, isPersistent: false);

        return Result.Success();
    }
}