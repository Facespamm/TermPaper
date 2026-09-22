namespace TermPaper.Application.Dto.ProjectDto;

public class UpdateProjectDto
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? DescriptionMd { get; set; }
    
    public DateTime? PeriodFrom { get; set; }
    
    public DateTime? PeriodTo { get; set; }

}