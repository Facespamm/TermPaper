using TermPaper.Application.Dto.AttributeValueDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class AttributeValueOptionMapper
{
    public static AttributeValueOption AddToEntity(AttributeValueDto dto,int maxOrder)
    {
        return new AttributeValueOption
        {
            AttributeId = dto.AttributeId,
            Value = dto.Value,
            Order = maxOrder + 1
        };
    }  
}