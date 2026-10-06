using System.Security.Claims;
using TermPaper.Application.Dto.SalesforceDto;
using TermPaper.Application.Dto.SupportTicketDto;
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
                try
                {
                    var token = await service.GetSalesforceToken();

                    var response = await service.CreateSalesforceAccount(token, dto, userId);

                    return Results.Ok(response);
                }
                catch (InvalidOperationException e)
                {
                    return Results.NotFound(new
                    {
                        message = e.Message
                    });
                }
            });
        app.MapPost("/api/support-tickets",
            async (ICrmService service, CreateTicketDto dto, ClaimsPrincipal user) =>
            {

            };

    }
}