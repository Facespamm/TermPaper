using System.ComponentModel.DataAnnotations;
using DataType = TermPaper.Enum.DataType;

namespace TermPaper.Application.Dto;

public class CreateAttributeDto
{
    public int CategoryId {get; set;}
    
    public string Name {get; set;}
    
    public DataType DataType {get; set;}
    
    public string Description {get; set;}
    
}