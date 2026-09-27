using TermPaper.Application.Dto.AttributesValueDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class AttributeValueOptionMapper
{
    public static AttributeValueOptionDto ToDto(AttributeValueOption entity)
    {
        return new AttributeValueOptionDto()
        {
            Id = entity.Id,
            Value = entity.Value,
            Order = entity.Order,
        };
    }

    public static AttributeValueOption ToEntity(AttributeValueAddDto addDto, int maxOrder)
    {
        return new AttributeValueOption
        {
            AttributeId = addDto.AttributeId,
            Value = addDto.Value,
            Order = maxOrder + 1
        };
    }

    public static AttributeValueOption UpdateEntity(UpdateAttributeValueDto dto, AttributeValueOption entity)
    {
        entity.AttributeId = dto.AttributeId ?? entity.AttributeId;
        entity.Value = dto.Value ?? entity.Value;
        return entity;
    }
}