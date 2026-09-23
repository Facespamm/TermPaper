using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
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
    private readonly string _baseUrl;

    public LoginService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ISenderEmail emailSender, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _baseUrl = configuration["AppSettings:BaseUrl"] ?? "http://localhost:5000";
    }
    public async Task<Result> LoginUsersAsync(string email, string password)
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
        {
            return Result.Failure(ErrorCode.UserNotFound);
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = QueryHelpers.AddQueryString($"{_baseUrl}/reset-password", new Dictionary<string, string?>{["email"] = email, ["token"] = token});
        await _emailSender.SendPasswordAsync(email, resetLink);
        return Result.Success();
    }
    
}