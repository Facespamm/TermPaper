using TermPaper.Application.Common;

namespace TermPaper.Interface;

public interface IExternalAuthService
{
    Task <Result> ExternalLoginAsync(string provider);
}