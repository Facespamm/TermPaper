namespace TermPaper.Application.Dto.UsersAttributesDto;

public class UpdateUserAttributeDto
{
    public int Id {get; set;}
    
    public int? AttributeId {get; set;}
    
    public string? Value {get; set;}
    
    public uint Version { get; set; }
}