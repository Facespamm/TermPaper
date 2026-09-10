
using TermPaper.Domain.Models;

namespace TermPaper.Application.Interface;

public interface IRegisterService
{
    Task<bool> UserExistsAsync (string email);
    Task<Result> RegisterAsync (string email, string password,string username);
    Task<Result>ConfirmEmailAsync (string email, string token);
}