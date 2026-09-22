namespace TermPaper.Domain.Models;

public class ProjectTags
{
    public int Id { get; set; }

    public string Tag { get; set; } = null!;
    
    public int ProjectId { get; set; }
    
    public Projects Project { get; set; } = null!;
    
}