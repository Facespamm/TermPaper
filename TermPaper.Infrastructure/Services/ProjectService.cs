using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.ProjectDto;
using TermPaper.Application.Dto.ProjectTagDto;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Mappers;

namespace TermPaper.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetProjectDto>> GetProjectList(string userId)
    {
        var entity = await _context.Projects
            .Where(x => x.UserId == userId)
            .Include(x => x.ProjectTags)
            .ToListAsync();

        return entity.Select(x => ProjectMapper.ToDto(x)).ToList();
    }

    public async Task<Result> CreateProject(CreateProjectDto createProjectDto)
    {
        var entity = ProjectMapper.ToEntity(createProjectDto);
        _context.Projects.Add(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProject(string userId, UpdateProjectDto updateProjectDto)
    {
        var entity = await _context.Projects.FindAsync(updateProjectDto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }

        if (entity.UserId != userId)
        {
            return Result.Failure(ErrorCode.AccessDenied);
        }

        var update = ProjectMapper.UpdateEntity(updateProjectDto, entity);
        _context.Projects.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProject(string userId, List<int> projectIds)
    {
        var entity = await _context.Projects
            .Where(x => projectIds.Contains(x.Id) && x.UserId == userId)
            .ToListAsync();

        _context.Projects.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<GetProjectTagDto>> GetProjectTag(string userId, int projectId)
    {
        var entity = await _context.ProjectTags
            .Where(x => x.ProjectId == projectId && x.Project.UserId == userId)
            .ToListAsync();

        return entity.Select(x => ProjectTagMapper.ToDto(x)).ToList();
    }

    public async Task<Result> CreateProjectTag(string userId, CreateProjectTagDto createProjectTagDto)
    {
        var ownsProject = await _context.Projects
            .AnyAsync(x => x.Id == createProjectTagDto.ProjectId && x.UserId == userId);
        if (!ownsProject)
        {
            return Result.Failure(ErrorCode.AccessDenied);
        }
        var entity = ProjectTagMapper.ToEntity(createProjectTagDto);
        _context.ProjectTags.Add(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProjectTag(string userId, UpdateProjectTagDto updateProjectTagDto)
    {
        var entity = await _context.ProjectTags
            .FirstOrDefaultAsync(x => x.Id == updateProjectTagDto.Id);

        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }

        var ownsProject = await _context.Projects
            .AnyAsync(x => x.Id == entity.ProjectId && x.UserId == userId);

        if (!ownsProject)
        {
            return Result.Failure(ErrorCode.AccessDenied);
        }

        var update = ProjectTagMapper.UpdateEntity(updateProjectTagDto, entity);
        _context.ProjectTags.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProjectTag(string userId, List<int> tagIds)
    {
        var entity = await _context.ProjectTags
            .Where(x => tagIds.Contains(x.Id) && x.Project.UserId == userId)
            .ToListAsync();

        _context.ProjectTags.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}