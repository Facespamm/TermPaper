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
        app.MapPost("/api/support-tickets-token",
            async (ICrmService service, CreateTicketDto dto, ClaimsPrincipal user) =>
            {
                var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) throw new InvalidOperationException($"Unable to load user with ID '{userId}'.");
                var create = await service.CreateSupportTicket(dto, userId);
                return Results.Ok(create);
            });
    }
}