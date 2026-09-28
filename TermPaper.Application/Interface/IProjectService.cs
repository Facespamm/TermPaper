using TermPaper.Application.Common;
using TermPaper.Application.Dto.ProjectDto;
using TermPaper.Application.Dto.ProjectTagDto;

namespace TermPaper.Application.Interface;

public interface IProjectService
{
    public Task<List<GetProjectDto>> GetProjectList(string userId);

    public Task<Result> CreateProject(CreateProjectDto createProjectDto);

    public Task<Result> UpdateProject(string userId, UpdateProjectDto updateProjectDto);

    public Task<Result> DeleteProject(string userId, List<int> projectIds);

    //ProjectTags

    public Task<List<GetProjectTagDto>> GetProjectTag(string userId, int projectId);

    public Task<Result> CreateProjectTag(string userId, CreateProjectTagDto createProjectTagDto);

    public Task<Result> UpdateProjectTag(string userId, UpdateProjectTagDto updateProjectTagDto);

    public Task<Result> DeleteProjectTag(string userId, List<int> tagIds);
}