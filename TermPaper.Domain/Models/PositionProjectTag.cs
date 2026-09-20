namespace TermPaper.Domain.Models;

public class PositionProjectTag
{
    public int Id { get; set; }
    
    public int PositionId { get; set; }
    
    public string Tag { get; set; }
    
    public Position Position { get; set; } = null!;
}