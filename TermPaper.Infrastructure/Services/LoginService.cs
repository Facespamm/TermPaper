using Microsoft.AspNetCore.Identity;
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

    public LoginService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result> LoginUsersAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){ return Result.Failure(ErrorCode.UserNotFound); }
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!result.Succeeded){ return Result.Failure(ErrorCode.PasswordsDoNotMatch); }
        if (result.IsNotAllowed)
        { return Result.Failure(ErrorCode.None); } 
        return Result.Success();
    }
}