using TermPaper.Enum;

namespace TermPaper.Application.Dto;

public class AttributeGetDto
{
    public int Id { get; set; }
    
    public int CategoryId {get; set;}
    
    public string Name {get; set;}
    
    public DataType DataType {get; set;}
    
    public string Description {get; set;}
    
    public bool IsBuiltIn {get; set;}
    
    public uint Version { get; set; }

}