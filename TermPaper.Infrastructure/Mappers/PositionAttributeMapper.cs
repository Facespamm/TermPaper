using TermPaper.Application.Dto.PositionAttributeDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class PositionAttributeMapper
{
    public static GetPositionAttributeDto GetPositionAttribute(int positionId)
    {
        return new GetPositionAttributeDto
        {
            Id = positionId,
            PositionId = positionId,
            AttributeId = positionId,
            Order = positionId,
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