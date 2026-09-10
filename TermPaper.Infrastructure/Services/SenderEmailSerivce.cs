using Microsoft.AspNetCore.Identity;
using Resend;
using TermPaper.Interface;
using TermPaper.Application.Interface;
using TermPaper.Domain.Models;
using TermPaper.Enum;

namespace TermPaper.Infrastructure.Services;

public class SenderEmailSerivce:ISenderEmail
{
    private readonly IResend _resend;
    private readonly UserManager<IdentityUser> _userManager;

    public SenderEmailSerivce(UserManager<IdentityUser> userManager, IResend resend)
    {
        _resend = resend;
        _userManager = userManager;
    }
    
    public async Task<Result> SendEmailAsync(string email,string token)
    {
        if (await _userManager.FindByEmailAsync(email) == null)
        {return Result.Failure(ErrorCode.UserNotFound);}
        var message = new EmailMessage();
        message.From = "onboarding@resend.dev";
        message.To.Add(email);
        message.Subject = "Confirmation email";
        message.HtmlBody = $"""
                            <h1>Confirmation email</h1>
                            <p>Click to confirm your email; if you received this email by mistake, please ignore it.
                            </p>
                            <a href="https://online-recruiter/confirm-email?email={email}&token={token}">Confirm email</a> 
                            """;
        await _resend.EmailSendAsync(message);
        return Result.Success();
    }
}