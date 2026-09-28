using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TermPaper.Application.Common;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;
using TermPaper.Infrastructure.Identity;

namespace TermPaper.Infrastructure.Services;

public class RegisterService : IRegisterService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ISenderEmailService _emailServiceSender;
    private readonly ILogger<RegisterService> _logger;
    private readonly string _baseUrl;

    public RegisterService(
        UserManager<AppUser> userManager,
        ISenderEmailService emailServiceSender,
        IConfiguration configuration,
        ILogger<RegisterService> logger)
    {
        _userManager = userManager;
        _emailServiceSender = emailServiceSender;
        _logger = logger;
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

        var roleResult = await _userManager.AddToRoleAsync(user, "Candidate");
        if (!roleResult.Succeeded)
        {
            _logger.LogError("Failed to assign role to user {UserId}: {Errors}",
                user.Id, string.Join("; ", roleResult.Errors.Select(e => e.Description)));
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmLink = QueryHelpers.AddQueryString($"{_baseUrl}/ConfirmEmailPage",
            new Dictionary<string, string?> { ["email"] = email, ["token"] = token });

        var send = await _emailServiceSender.SendEmailAsync(email, confirmLink);
        if (!send.IsSuccess)
        {
            _logger.LogError("Confirmation email was not sent to user {UserId}. Rolling back registration.", user.Id);
            await _userManager.DeleteAsync(user);

            return Result.Failure(
                ErrorCode.EmailSendFailed,
                "We could not send the confirmation email. Please try again later.");
        }

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