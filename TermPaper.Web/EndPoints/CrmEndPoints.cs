using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Application.Interface;
using TermPaper.Infrastructure.Services;

namespace TermPaper.EndPoints;

public class CrmEndPoints
{
    public void MapCrmEndpoints(WebApplication app)
    {
        app.MapPost("/api/salesforce/token",
            async (ICrmService service, SalesforceAccountCreateDto dto, string userId) =>
            {
                var token = await service.GetSalesforceAuf();
                var response = await service.CreateAccountSalesforce(token, dto, userId);
                return response;
            });
    }
}