namespace TermPaper.Infrastructure.Settings;

public class GoogleAuthenticationSettings
{
    public const string SectionName = "Authentication:Google";
    public string ClientId { get; set; }
    public string ClientSecret { get;set; }
}