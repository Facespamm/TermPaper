using Microsoft.AspNetCore.Identity;
using TermPaper.Domain.Enum;
namespace TermPaper.Infrastructure.Identity;

public class AppUser:IdentityUser
{
    public AppLocale Locale { get; set; }
    
    public AppTheme Theme { get; set; }
    
    public string? SalesforceAccountId { get; set; }
    
    public string? SalesforceContactId { get; set; }
    
}