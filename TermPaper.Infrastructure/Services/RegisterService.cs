using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Resend;
using TermPaper.Domain.Models;
using TermPaper.Enum;
using TermPaper.Infrastructure.Services;

namespace TermPaper.Services;

public class RegisterService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailSender  _emailSender;

    public RegisterService(UserManager<IdentityUser> userManager,IEmailSender  emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<Result> RegisterUserAsync( string email, string password, string confirmPassword,string userName)
    {
        var existingUser  = await _userManager.FindByEmailAsync(email);
        if ( existingUser != null){ return  Result.Failure(ErrorCode.UserNotFound); }
        if (password != confirmPassword){return Result.Failure(ErrorCode.PasswordsDoNotMatch); }
        var user = new IdentityUser
        {
            UserName = userName,
            Email = email,
        };
        var result = await _userManager.CreateAsync(user,password);
        if (!result.Succeeded) {return Result.Failure(ErrorCode.PasswordsDoNotMatch);}

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
       // var send = await _emailSender.SendEmailAsync(email,token);
        return Result.Success();
    }
    
    public async Task<IdentityResult> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){ return IdentityResult.Failed(); }
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded){ return IdentityResult.Failed(); }

        return IdentityResult.Success;
    }


}