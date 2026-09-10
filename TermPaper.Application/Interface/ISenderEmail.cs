using TermPaper.Domain.Models;

namespace TermPaper.Application.Interface;

public interface ISenderEmail
{
    Task<Result> SendEmailAsync (string email, string token);
}