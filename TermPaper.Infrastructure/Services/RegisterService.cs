using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using TermPaper.Application.Common;
using TermPaper.Enum;
using TermPaper.Application.Interface;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Services;

public class RegisterService : IRegisterService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ISenderEmail _emailSender;
    private readonly string _baseUrl;

    public RegisterService(UserManager<AppUser> userManager, ISenderEmail emailSender, IConfiguration configuration)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _baseUrl = configuration["AppSettings:BaseUrl"] ?? "http://localhost:5000";
    }
    public async Task<Result> RegisterUserAsync(string email, string password, string confirmPassword, string userName)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null) { return Result.Failure(ErrorCode.UserAlreadyExists); }

        if (password != confirmPassword) { return Result.Failure(ErrorCode.PasswordsDoNotMatch); }

        var user = new AppUser
        {
            UserName = userName,
            Email = email,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var message = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(ErrorCode.ValidationFailed, message);
        }

        await _userManager.AddToRoleAsync(user, "Candidate");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmLink = QueryHelpers.AddQueryString($"{_baseUrl}/ConfirmEmailPage", 
            new Dictionary<string, string?>{["email"] = email,["token"] = token});
        var send = await _emailSender.SendEmailAsync(email, confirmLink);

        return Result.Success();
    }

    public async Task<Result> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) { return Result.Failure(ErrorCode.UserNotFound); }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded) { return Result.Failure(ErrorCode.UserNotFound); }

        return Result.Success();
    }
}