using TermPaper.Enum;

namespace TermPaper.Domain.Models;

public class Attributes
{
    public int Id { get; set; }
    
    public int CategoryId {get; set;}
    
    public string Name {get; set;}
    
    public DataType DataType {get; set;}
    
    public string Description {get; set;}
    
    public bool IsBuiltIn {get; set;}
    
    public AttributeCategory Category {get; set;}
    
    public List<AttributeValueOption> AttributeValueOptions { get; set; } = new();
    
    public List<UserAttributes> UserAttributes { get; set; } = new();
    
    
    

    
}