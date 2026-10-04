using TermPaper.Application.Common;
using TermPaper.Application.Dto.SalesforceDto;
namespace TermPaper.Application.Interface;

public interface ICrmService
{
    public Dictionary<string, string> SalesforceVerificationData();

    public Task<string> GetSalesforceAuf();

    public string GetSalesforceToken(string authResponse);

    public Task<Result> CreateAccountSalesforce(string token, SalesforceAccountCreateDto dto, string userId);

    public Task<SalesforceCompositeRequestDto> CreateDataAccountSalesforce(SalesforceAccountCreateDto dto,
        string userId);


}