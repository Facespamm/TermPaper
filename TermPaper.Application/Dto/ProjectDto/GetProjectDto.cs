using TermPaper.Application.Dto.ProjectTagDto;

namespace TermPaper.Application.Dto.ProjectDto;

public class GetProjectDto
{
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? DescriptionMd { get; set; }
    
    public string UserId { get; set; }
    
    public DateTime? PeriodFrom { get; set; }
    
    public DateTime? PeriodTo { get; set; }

    public List<GetProjectTagDto> Tags { get; set; } = new();

}