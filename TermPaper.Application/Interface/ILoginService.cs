using TermPaper.Application.Common;
using TermPaper.Application.Common;

namespace TermPaper.Application.Interface;

public interface ILoginService
{
    Task<Result> LoginUsersAsync (string email, string password);
    Task<Result> ChangePasswordAsync (string email);
    
}