using TermPaper.Application.Common;
using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Application.Dto.SupportTicketDto;

namespace TermPaper.Application.Interface;

public interface ICrmService
{
    // Salesforce
    Dictionary<string, string> GetSalesforceAuthData();
    Task<string> GetSalesforceToken();
    string ParseAccessToken(string authResponse);
    Task<Result> CreateSalesforceAccount(string token, SalesforceAccountCreateDto dto, string userId);
    Task<SalesforceCompositeRequestDto> BuildSalesforceRequest(SalesforceAccountCreateDto dto, string userId);

    // Power Automate (OneDrive через Microsoft Graph)
    Dictionary<string, string> GetEntraAuthData();
    Task<string> GetGraphToken();
    Task<Result> CreateSupportTicket(CreateTicketDto createTicketDto, string userId);
}