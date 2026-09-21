using TermPaper.Application.Dto.PositionAttributeDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class PositionAttributeMapper
{
    public static GetPositionAttributeDto GetPositionAttribute(PositionAttribute positionAttribute)
    {
        return new GetPositionAttributeDto
        {
            Id = positionAttribute.Id,
            PositionId = positionAttribute.PositionId,
            AttributeId = positionAttribute.AttributeId,
            Order = positionAttribute.Order,
        };
    }

    public static PositionAttribute CreatePositionAttributeDto(CreatePositionAttributeDto dto)
    {
        return new PositionAttribute()
        {
            PositionId = dto.PositionId,
            AttributeId = dto.AttributeId,
            Order = dto.Order,
        };
    }
}