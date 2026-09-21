using TermPaper.Domain.Models;

namespace TermPaper.Application.Dto.ProjectTagDto;

public class GetProjectTagDto
{
    public int Id { get; set; }
    
    public string Tag { get; set; }
    
    public int ProjectId { get; set; }
    
    public Projects Project { get; set; } = null!;

}