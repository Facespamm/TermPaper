
using TermPaper.Application.Common;

namespace TermPaper.Application.Interface;

public interface IRegisterService
{
    //Task<bool> UserExistsAsync (string email);
    Task<Result> RegisterUserAsync (string email, string password, string confirmPassword,string userName);
    Task<Result>ConfirmEmailAsync (string email, string token);
}