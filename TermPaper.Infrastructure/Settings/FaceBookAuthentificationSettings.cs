namespace TermPaper.Infrastructure.Settings;

public class FaceBookAuthentificationSettings
{
    public const string SectionName = "Authentication:Facebook";

    public string AppId { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
}