using TermPaper.Application.Dto.AttributesValueDto;
using TermPaper.Domain.Enum;
namespace TermPaper.Application.Dto.AttributeDto;

public class AttributeGetDto
{
    public int Id { get; set; }
    
    public int CategoryId {get; set;}
    
    public string Name {get; set;}
    
    public DataType DataType {get; set;}
    
    public string Description {get; set;}
    
    public bool IsBuiltIn {get; set;}
    
    public List<AttributeValueOptionDto> AttributeValueOptions { get; set; } = [];
    
    public uint Version { get; set; }

}