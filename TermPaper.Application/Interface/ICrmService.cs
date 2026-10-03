using TermPaper.Application.Common;
using TermPaper.Application.Dto.SalesforceDto;
namespace TermPaper.Application.Interface;

public interface ICrmService
{
    public Dictionary<string,string> SalesforceVerificationData();

    public Task<string> GetSalesforceToken();

    public Task<Result> CreateAccountSalesforce(string token);
}