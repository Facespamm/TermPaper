using TermPaper.Application.Dto.ProjectTagDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class ProjectTagMapper
{
    public static GetProjectTagDto GetProjectTagData(ProjectTags tags)
    {
        return new GetProjectTagDto()
        {
            Id = tags.Id,
            ProjectId = tags.ProjectId,
            Tag =  tags.Tag,
        };
    }

    public static ProjectTags CreateProjectTagsData(CreateProjectTagDto dto)
    {
        return new ProjectTags()
        {
            ProjectId = dto.ProjectId,
            Tag = dto.Tag,
        };
    }

    public static ProjectTags UpdateProjectTagsData(UpdateProjectTagDto tagDto, ProjectTags projectData)
    {
        projectData.ProjectId = tagDto.ProjectId ?? projectData.ProjectId;
        projectData.Tag = tagDto.Tag ?? projectData.Tag;
        return projectData;
    }
}
