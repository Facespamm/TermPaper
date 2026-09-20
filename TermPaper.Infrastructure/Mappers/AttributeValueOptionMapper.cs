using TermPaper.Application.Dto.AttributeValueDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class AttributeValueOptionMapper
{
    public static AttributeValueOption AddToEntity(AttributeValueAddDto addDto,int maxOrder)
    {
        return new AttributeValueOption
        {
            AttributeId = addDto.AttributeId,
            Value = addDto.Value,
            Order = maxOrder + 1
        };
    }

    public static AttributeValueOption UpdateToEntity(UpdateAttributeValueDto dto,AttributeValueOption entity)
    {
        return new AttributeValueOption
        {
            AttributeId = dto.AttributeId ?? entity.AttributeId,
            Value = dto.Value ?? entity.Value,
        };
    }
}