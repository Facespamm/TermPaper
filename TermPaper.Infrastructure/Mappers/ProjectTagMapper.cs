using TermPaper.Application.Dto.PositionProjectTagDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class PositionProjectTagMapper
{
    public static GetPositionProjectTagDto ToDto(PositionProjectTag positionProjectTag)
    {
        return new GetPositionProjectTagDto()
        {
            Id = positionProjectTag.Id,
            PositionId = positionProjectTag.PositionId,
            Tag = positionProjectTag.Tag
        };
    }

    public static PositionProjectTag ToEntity(CreatePositionProjectTagDto dto)
    {
        return new PositionProjectTag()
        {
            PositionId = dto.PositionId,
            Tag = dto.Tag
        };
    }
}