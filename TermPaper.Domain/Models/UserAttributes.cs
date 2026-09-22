namespace TermPaper.Domain.Models;

public class UserAttributes
{
    public int Id {get; set;}
    
    public int AttributeId {get; set;}
    
    public Attributes Attributes {get; set;}
    
    public string UserId  {get; set;}
    
    public string Value {get; set;}
    
    public uint Version {get; set;}
}