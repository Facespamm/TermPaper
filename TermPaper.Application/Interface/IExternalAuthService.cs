using TermPaper.Application.Common;

namespace TermPaper.Application.Interface;

public interface IExternalAuthService
{
    Task <Result> ExternalLoginAsync();
}