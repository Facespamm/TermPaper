using TermPaper.Application.Dto.ProjectDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class ProjectMapper
{
    public static GetProjectDto ToDto(Projects project)
    {
        return new GetProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
            DescriptionMd = project.DescriptionMd,
            UserId = project.UserId,
            PeriodFrom = project.PeriodFrom,
            PeriodTo = project.PeriodTo,
            Tags = project.ProjectTags.Select(x => ProjectTagMapper.ToDto(x)).ToList(),
        };
    }

    public static Projects ToEntity(CreateProjectDto dto)
    {
        return new Projects()
        {
            Name = dto.Name,
            DescriptionMd = dto.DescriptionMd,
            UserId = dto.UserId,
            PeriodFrom = dto.PeriodFrom.HasValue
                ? DateTime.SpecifyKind(dto.PeriodFrom.Value, DateTimeKind.Utc)
                : null,
            PeriodTo = dto.PeriodTo.HasValue
                ? DateTime.SpecifyKind(dto.PeriodTo.Value, DateTimeKind.Utc)
                : null,
        };
    }

    public static Projects UpdateEntity(UpdateProjectDto dto, Projects project)
    {
        project.DescriptionMd = dto.DescriptionMd ?? project.DescriptionMd;
        project.Name = dto.Name ?? project.Name;
        project.PeriodFrom = dto.PeriodFrom ?? project.PeriodFrom;
        project.PeriodTo = dto.PeriodTo ?? project.PeriodTo;
        return project;
    }
}