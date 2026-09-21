using TermPaper.Application.Dto.PositionProjectTagDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class PositionProjectTagMapper
{
    public static GetPositionProjectTagDto GetPositionProjectTag(PositionProjectTag position)
    {
        return new GetPositionProjectTagDto()
        {
            Id = position.Id,
            PositionId = position.PositionId,
            Tag = position.Tag
        };
    }

    public static PositionProjectTag CreatePositionTag(CreatePositionProjectTagDto dto)
    {
        return new PositionProjectTag()
        {
            PositionId = dto.PositionId,
            Tag = dto.Tag
        };
    }
}