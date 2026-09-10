using Microsoft.AspNetCore.Identity;
using Resend;
using TermPaper.Interface;
using TermPaper.Application.Interface;
using TermPaper.Application.Common;
using TermPaper.Enum;

namespace TermPaper.Infrastructure.Services;

public class SenderEmailSerivce:ISenderEmail
{
    private readonly IResend _resend;

    public SenderEmailSerivce(IResend resend)
    {
        _resend = resend;
    }
    
    public async Task<Result> SendEmailAsync(string email,string link)
    {
        var message = new EmailMessage();
        message.From = "onboarding@resend.dev";
        message.To.Add(email);
        message.Subject = "Confirmation email";
        message.HtmlBody = $"""
                            <h1>Confirmation email</h1>
                            <p>Click to confirm your email; if you received this email by mistake, please ignore it.
                            </p>
                            <a href="{link}">Confirm email</a> 
                            """;
        await _resend.EmailSendAsync(message);
        return Result.Success();
    }
}