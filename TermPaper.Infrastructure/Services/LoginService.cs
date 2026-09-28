using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TermPaper.Application.Common;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;
using TermPaper.Infrastructure.Identity;

namespace TermPaper.Infrastructure.Services;

public class LoginService : ILoginService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ISenderEmailService _emailServiceSender;
    private readonly ILogger<LoginService> _logger;
    private readonly string _baseUrl;

    public LoginService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ISenderEmailService emailServiceSender,
        IConfiguration configuration,
        ILogger<LoginService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailServiceSender = emailServiceSender;
        _logger = logger;
        _baseUrl = configuration["AppSettings:BaseUrl"] ?? "http://localhost:5000";
    }

    public async Task<Result> LoginUsersAsync(string email, string password, bool isPersistent = false)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) { return Result.Failure(ErrorCode.UserNotFound); }

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure: false);

        if (result.IsNotAllowed) { return Result.Failure(ErrorCode.EmailNotConfirmed); }
        if (!result.Succeeded) { return Result.Failure(ErrorCode.PasswordsDoNotMatch); }

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
        var resetLink = QueryHelpers.AddQueryString($"{_baseUrl}/reset-password",
            new Dictionary<string, string?> { ["email"] = email, ["token"] = token });

        var send = await _emailServiceSender.SendPasswordAsync(email, resetLink);
        if (!send.IsSuccess)
        {
            _logger.LogError("Password reset email was not sent to user {UserId}", user.Id);
            return Result.Failure(ErrorCode.EmailSendFailed);
        }

        return Result.Success();
    }
}