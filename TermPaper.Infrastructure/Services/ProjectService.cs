using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.ProjectDto;
using TermPaper.Application.Dto.ProjectTagDto;
using TermPaper.Application.Interface;
using TermPaper.Domain.Models;
using TermPaper.Enum;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Mappers;

namespace TermPaper.Infrastructure.Services;

public class ProjectService :IProjectService
{
    private readonly AppDbContext _context;
    
    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetProjectDto>> GetProjectList(string userId)
    {
        var entity = await _context.Projects.Where(x => x.UserId == userId).ToListAsync();
        
        return entity.Select(x => ProjectMapper.GetProjectData(x)).ToList();
    }

    public async Task<Result> CreateProject(CreateProjectDto createProjectDto)
    {
        var entity = ProjectMapper.CreateProjectData(createProjectDto);
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProject(UpdateProjectDto updateProjectDto)
    {
        var entity = await _context.Projects.FindAsync(updateProjectDto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }
        var update = ProjectMapper.UpdateProjectData(updateProjectDto, entity);
        _context.Projects.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProject(List<int> projectIds)
    {
        var entity = await _context.Projects.Where(x => projectIds.Contains(x.Id)).ToListAsync();
        _context.Projects.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<GetProjectTagDto>> GetProjectTag(int projectId)
    {
        var entity = await _context.ProjectTags.Where(x => x.ProjectId == projectId).ToListAsync();
        return  entity.Select(x=>ProjectTagMapper.GetProjectTagData(x)).ToList();
    }

    public async Task<Result> CreateProjectTag(CreateProjectTagDto createProjectTagDto)
    {
        var entity = ProjectTagMapper.CreateProjectTagsData(createProjectTagDto);
        _context.ProjectTags.Add(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProjectTag(UpdateProjectTagDto updateProjectTagDto)
    {
        var entity = await _context.ProjectTags.FindAsync(updateProjectTagDto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }
        var update = ProjectTagMapper.UpdateProjectTagsData(updateProjectTagDto, entity); 
        _context.ProjectTags.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProjectTag(List<int> projectIds)
    {
        var entity = await _context.ProjectTags.Where(x => projectIds.Contains(x.ProjectId)).ToListAsync();
        _context.ProjectTags.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}