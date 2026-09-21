namespace TermPaper.Application.Dto.ProjectTagDto;

public class UpdateProjectTagDto
{
    public int Id { get; set; }
    
    public string? Tag { get; set; }
    
    public int? ProjectId { get; set; }
}