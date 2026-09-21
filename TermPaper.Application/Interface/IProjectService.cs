using TermPaper.Application.Common;
using TermPaper.Application.Dto.ProjectDto;
using TermPaper.Application.Dto.ProjectTagDto;

namespace TermPaper.Application.Interface;

public interface IProjectService
{
    public Task<GetProjectDto> GetProjects(int userId);
    
    public Task<Result> CreateProject(CreateProjectDto createProjectDto);
    
    public Task<Result> UpdateProject(UpdateProjectDto updateProjectDto);
    
    public Task<Result> DeleteProject(int projectId);
    
    //ProjectTags
    
    public Task<GetProjectTagDto> GetProject(int projectId);
    
    public Task<Result> AddTag(CreateProjectTagDto createProjectTagDto);
    
    public Task<Result> UpdateTag(UpdateProjectTagDto updateProjectTagDto);
    
    public Task<Result> DeleteTag(List<int> tagId);
}