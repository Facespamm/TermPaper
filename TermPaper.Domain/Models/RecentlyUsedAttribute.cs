namespace TermPaper.Domain.Models;

public class RecentlyUsedAttribute
{
    public int Id { get; set; }

    public int AttributeId { get; set; }
    
    public Attributes Attribute { get; set; } = null!;

    public string UserId { get; set; } = "";

    public DateTime LastUsedAt { get; set; }
}