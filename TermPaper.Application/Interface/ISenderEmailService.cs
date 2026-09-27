using TermPaper.Application.Common;

namespace TermPaper.Application.Interface;

public interface ISenderEmailService
{
    Task<Result> SendEmailAsync (string email, string token);
    Task<Result> SendPasswordAsync(string email, string token);
}