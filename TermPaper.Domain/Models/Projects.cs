namespace TermPaper.Domain.Models;

public class Projects
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? DescriptionMd { get; set; }

    public string UserId { get; set; } = null!;
    
    public DateTime? PeriodFrom { get; set; }
    
    public DateTime? PeriodTo { get; set; }

    public List<ProjectTags> ProjectTags { get; set; }
}