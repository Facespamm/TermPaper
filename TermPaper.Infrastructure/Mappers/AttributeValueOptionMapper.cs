using TermPaper.Application.Dto.AttributeValueDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class AttributeValueOptionMapper
{

    public static AttributeValueOptionDto GetValueOption(AttributeValueOption entity)
    {
        return new AttributeValueOptionDto()
        {
            Id = entity.Id,
            Value = entity.Value,
            Order = entity.Order,
        };
    }
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
        entity.AttributeId = dto.AttributeId ?? entity.AttributeId;
        entity.Value = dto.Value ?? entity.Value;
        return entity;
    }
}