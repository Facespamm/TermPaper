namespace TermPaper.Application.Dto.UsersAttributesDto;

public class GetUserAttributeDto
{
    public int Id {get; set;}
    
    public int AttributeId {get; set;}
    
    public string UserId  {get; set;}
    
    public string Value {get; set;}
    
    public uint Version { get; set; }
    
    public AttributeGetDto Attribute { get; set; }
    
}