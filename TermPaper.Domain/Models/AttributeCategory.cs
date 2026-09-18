namespace TermPaper.Domain.Models;

public class AttributeCategory
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Attributes> AttributesList { get; set; } = new();

    
}   