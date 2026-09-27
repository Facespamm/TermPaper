using TermPaper.Application.Dto.ProjectTagDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class ProjectTagMapper   // ← вот так
{
    public static GetProjectTagDto ToDto(ProjectTags tag)
    {
        return new GetProjectTagDto()
        {
            Id = tag.Id,
            ProjectId = tag.ProjectId,
            Tag = tag.Tag,
        };
    }

    public static ProjectTags ToEntity(CreateProjectTagDto dto)
    {
        return new ProjectTags()
        {
            ProjectId = dto.ProjectId,
            Tag = dto.Tag,
        };
    }

    public static ProjectTags UpdateEntity(UpdateProjectTagDto dto, ProjectTags tag)
    {
        tag.ProjectId = dto.ProjectId ?? tag.ProjectId;
        tag.Tag = dto.Tag ?? tag.Tag;
        return tag;
    }
}