using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using TermPaper.Application.Common;
using TermPaper.Enum;
using TermPaper.Application.Interface;
using Microsoft.AspNetCore.WebUtilities;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Services;

public class RegisterService:IRegisterService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ISenderEmail  _emailSender;

    public RegisterService(UserManager<AppUser> userManager,ISenderEmail emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<Result> RegisterUserAsync( string email, string password, string confirmPassword,string userName)
    {
        var existingUser  = await _userManager.FindByEmailAsync(email);
        if ( existingUser != null){ return  Result.Failure(ErrorCode.UserNotFound); }
        if (password != confirmPassword){return Result.Failure(ErrorCode.PasswordsDoNotMatch); }
        var user = new AppUser
        {
            UserName = userName,
            Email = email,
        };
        var result = await _userManager.CreateAsync(user,password);
        if (!result.Succeeded) {return Result.Failure(ErrorCode.PasswordsDoNotMatch);}
        await _userManager.AddToRoleAsync(user,"Candidate");
        var token =  await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmLink = QueryHelpers.AddQueryString("http://localhost:5000/ConfirmEmailPage",new Dictionary<string, string?>{["email"] = email,["token"] = token});
        var send = await _emailSender.SendEmailAsync(email,confirmLink);
        return Result.Success();
    }
    
    public async Task<Result> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null){ return Result.Failure(ErrorCode.UserNotFound); }
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded){ return Result.Failure(ErrorCode.UserNotFound); }

        return Result.Success();
    }

}