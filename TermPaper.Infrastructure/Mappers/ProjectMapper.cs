using TermPaper.Application.Dto.ProjectDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class ProjectMapper
{
    public static GetProjectDto GetProjectData(Projects project)
    {
        return new GetProjectDto()
        {
            Id = project.Id,
            Name = project.Name,
            DescriptionMd = project.DescriptionMd,
            UserId = project.UserId,
            PeriodFrom = project.PeriodFrom,
            PeriodTo = project.PeriodTo,
        };
    }

    public static Projects CreateProjectData(CreateProjectDto dto)
    {
        return new Projects()
        {
            Name = dto.Name,
            DescriptionMd = dto.DescriptionMd,
            UserId = dto.UserId,
            PeriodFrom = dto.PeriodFrom,
            PeriodTo = dto.PeriodTo,
        };
    }

    public static Projects UpdateProjectData(UpdateProjectDto project, Projects projectData)
    {
        projectData.DescriptionMd = project.DescriptionMd ?? projectData.DescriptionMd;
        projectData.Name = project.Name  ?? projectData.Name;
        projectData.PeriodFrom= project.PeriodFrom  ?? projectData.PeriodFrom;
        projectData.PeriodTo = project.PeriodTo   ?? projectData.PeriodTo;
        return projectData;
    }
}