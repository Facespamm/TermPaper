using Microsoft.AspNetCore.Identity;
using TermPaper.Enum;

namespace TermPaper.Models;

public class AppUser:IdentityUser
{
    public AppLocale Locale { get; set; }
    
    public AppTheme Theme { get; set; }
}