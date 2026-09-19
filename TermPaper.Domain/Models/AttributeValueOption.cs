namespace TermPaper.Domain.Models;

public class AttributeValueOption
{
    public int Id {get; set;}
    
    public int AttributeId {get; set;}
    
    public string Value {get; set;}

    public Attributes Attribute { get; set; } = null!;
    
    public int? Order {get; set;}
}