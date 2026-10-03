using TermPaper.Application.Interface;
using TermPaper.Infrastructure.Services;

namespace TermPaper.EndPoints;

public class CrmEndPoints
{
    public void MapCrmEndpoints(WebApplication app)
    {
        // app.MapPost("/api/salesforce/token",(ICrmService service) =>
        // {
        //   var token=  service.GetSalesforceToken();
        // })
    }
}