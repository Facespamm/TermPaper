using DataType = TermPaper.Domain.Enum.DataType;

namespace TermPaper.Application.Dto.AttributeDto;

public class CreateAttributeDto
{
    public int CategoryId {get; set;}
    
    public string Name {get; set;}
    
    public DataType DataType {get; set;}
    
    public string Description {get; set;}
    
}