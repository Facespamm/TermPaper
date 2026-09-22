namespace TermPaper.Domain.Models;

public class Position
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? ShortDescription { get; set; }
    
    public bool IsPublic  { get; set; }
    
    public int? MaxProjectsCount  { get; set; }
    
    public string? CompanyName { get; set; }
    
    public string? Level { get; set; }
}