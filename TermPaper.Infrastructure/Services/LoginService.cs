using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Resend;
using TermPaper.Application.Interface;
using TermPaper.Application.Common;
using TermPaper.Enum;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Services;

public class LoginService:ILoginService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ISenderEmail _emailSender;

    public LoginService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ISenderEmail emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    public async Task<Result> LoginUsersAsync(string email, string password, bool isPersistent = false)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){ return Result.Failure(ErrorCode.UserNotFound); }
        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure: false);
        if (!result.Succeeded){ return Result.Failure(ErrorCode.PasswordsDoNotMatch); }
        if (result.IsNotAllowed)
        { return Result.Failure(ErrorCode.None); } 
        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        { Result.Failure(ErrorCode.UserNotFound); }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = QueryHelpers.AddQueryString("https://online-recruiter/reset-password", new Dictionary<string, string?>{["email"] = email, ["token"] = token});
        await _emailSender.SendPasswordAsync(email,resetLink);
        return Result.Success();
    }
}