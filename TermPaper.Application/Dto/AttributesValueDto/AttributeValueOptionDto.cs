using TermPaper.Domain.Models;

namespace TermPaper.Application.Dto.AttributeValueDto;

public class AttributeValueOptionDto
{
    public int Id { get; set; }
    public string Value { get; set; } = "";
    public int? Order { get; set; }
    
    
}   