namespace TermPaper.Domain.Models;

public class AttributeEnumValue
{
    public int Id {get; set;}
    
    public int AtributeId {get; set;}
    
    public string Value {get; set;}

    public Attributes Attribute { get; set; } = null!;
    
    public int Order {get; set;}
}