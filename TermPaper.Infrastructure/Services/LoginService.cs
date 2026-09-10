using Microsoft.AspNetCore.Identity;
using Resend;
using TermPaper.Application.Interface;
using TermPaper.Application.Common;
using TermPaper.Enum;

namespace TermPaper.Infrastructure.Services;

public class LoginService:ILoginService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result> LoginUsersAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){ return Result.Failure(ErrorCode.UserNotFound); }
        var result = await _signInManager.PasswordSignInAsync(user,password, false, false);
        if (!result.Succeeded){ return Result.Failure(ErrorCode.PasswordsDoNotMatch); }
        if (result.IsNotAllowed)
        {
            return Result.Failure(ErrorCode.None);
        }
        if (!result.Succeeded)
        {
            return Result.Failure(ErrorCode.None);
        }

        return Result.Success();
    }
}