using TermPaper.Application.Common;
using TermPaper.Application.Dto.ProjectDto;
using TermPaper.Application.Dto.ProjectTagDto;

namespace TermPaper.Application.Interface;

public interface IProjectService
{
    public Task<List<GetProjectDto>> GetProjectList(string userId);
    
    public Task<Result> CreateProject(CreateProjectDto createProjectDto);
    
    public Task<Result> UpdateProject(UpdateProjectDto updateProjectDto,string currentUserId);
    
    public Task<Result> DeleteProject(List<int> projectIds, string currentUserId);
    
    //ProjectTags
    
    public Task<List<GetProjectTagDto>> GetProjectTag(int projectId);
    
    public Task<Result> CreateProjectTag(CreateProjectTagDto createProjectTagDto);
    
    public Task<Result> UpdateProjectTag(UpdateProjectTagDto updateProjectTagDto);
    
    public Task<Result> DeleteProjectTag(List<int> tagId);
}