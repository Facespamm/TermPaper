namespace TermPaper.Application.Dto.PositionDto;

public class CreatePositionDto
{
    public string Title { get; set; }
    
    public string ShortDescription { get; set; }
    
    public bool IsPublic  { get; set; }
    
    public int MaxProjectsCount  { get; set; }
    
    public string? CompanyName { get; set; }
    
    public string? Level { get; set; }
}