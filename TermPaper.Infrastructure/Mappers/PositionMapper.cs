using TermPaper.Application.Dto.PositionDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class PositionMapper
{
    public static GetPositionDto GetToPosition(Position position)
    {
        return new GetPositionDto()
        {
            Id = position.Id,
            Title = position.Title,
            ShortDescription = position.ShortDescription,
        };
    }

    public static GetPositionInfoDto GetPositionToInfo(Position position)
    {
        return new GetPositionInfoDto
        {
            Id = position.Id,
            Title = position.Title,
            ShortDescription = position.ShortDescription,
            CompanyName = position.CompanyName,
            IsPublic = position.IsPublic,
            MaxProjectsCount = position.MaxProjectsCount,
            Level = position.Level,
        };
    }
    
    public static Position CreateToPosition(CreatePositionDto dto)
    {
        return new Position
        {
            IsPublic = dto.IsPublic,
            MaxProjectsCount = dto.MaxProjectsCount,
            CompanyName = dto.CompanyName,
            ShortDescription = dto.ShortDescription,
            Title = dto.Title,
            Level = dto.Level
        };
    }
    
    public static Position UpdateToEntity(UpdatePositionDto dto, Position position)
    {
        position.CompanyName = dto.CompanyName ??  position.CompanyName;
        position.Level = dto.Level  ??  position.Level;
        position.MaxProjectsCount = dto.MaxProjectsCount ?? position.MaxProjectsCount;
        position.ShortDescription = dto.ShortDescription ?? position.ShortDescription;
        position.Title = dto.Title ?? position.Title;
        return position;
    }
}