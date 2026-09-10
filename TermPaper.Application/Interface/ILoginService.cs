using TermPaper.Domain.Models;

namespace TermPaper.Application.Interface;

public interface ILoginService
{
    Task<Result> LoginAsync (string email, string password);
    Task<string>GeneratePasswordResetTokenAsync (string email);
    
}