using Microsoft.Extensions.Logging;
using Resend;
using TermPaper.Application.Common;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;

namespace TermPaper.Infrastructure.Services;

public class SenderEmailService : ISenderEmailService
{
    private readonly IResend _resend;
    private readonly ILogger<SenderEmailService> _logger;

    public SenderEmailService(IResend resend, ILogger<SenderEmailService> logger)
    {
        _resend = resend;
        _logger = logger;
    }

    public async Task<Result> SendEmailAsync(string email, string link)
    {
        var message = new EmailMessage();
        message.From = "noreply@xyzs.click";
        message.To.Add(email);
        message.Subject = "Confirmation email";
        message.HtmlBody = $"""
                            <h1>Confirmation email</h1>
                            <p>Click to confirm your email; if you received this email by mistake, please ignore it.
                            </p>
                            <a href="{link}">Confirm email</a> 
                            """;

        return await SendAsync(message);
    }

    public async Task<Result> SendPasswordAsync(string email, string link)
    {
        var message = new EmailMessage();
        message.From = "noreply@xyzs.click";
        message.To.Add(email);
        message.Subject = "Confirmation code to password";
        message.HtmlBody = $"""
                            <h1>Confirmation code</h1>
                            <p>Enter code to app.</p>
                            <a href="{link}">Confirm password</a> 
                            """;

        return await SendAsync(message);
    }

    private async Task<Result> SendAsync(EmailMessage message)
    {
        try
        {
            await _resend.EmailSendAsync(message);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via Resend. Subject: {Subject}", message.Subject);
            return Result.Failure(ErrorCode.EmailSendFailed);
        }
    }
}