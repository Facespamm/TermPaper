using TermPaper.Application.Common;

namespace TermPaper.Application.Interface;

public interface ISenderEmail
{
    Task<Result> SendEmailAsync (string email, string token);
}