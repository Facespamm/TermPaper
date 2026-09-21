using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.PositionAttributeDto;
using TermPaper.Application.Dto.PositionDto;
using TermPaper.Application.Dto.PositionProjectTagDto;
using TermPaper.Enum;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Mappers;
using System.Linq;
using TermPaper.Application.Dto.PositionAccessRuleDto;
using TermPaper.Application.Interface;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Services;

public class PositionService:IPositionService
{
    private readonly AppDbContext _context;
    
    public PositionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetPositionDto>> GetPositions(string search)
    {
        var records = await _context.Positions.Where(x => x.Title == search || x.ShortDescription == search)
            .ToListAsync(); 
        return records.Select(x=>PositionMapper.GetToPosition(x)).ToList();
    }

    public async Task<GetPositionInfoDto> GetPositionInfo(int id)
    {
        var info = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
        if (info == null)
        {
            return new GetPositionInfoDto();
        }
        return PositionMapper.GetPositionToInfo(info);
    }

    public async Task<Result> CreatePosition(CreatePositionDto dto)
    {
        var entity = PositionMapper.CreateToPosition(dto);
        
        await _context.Positions.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdatePosition(UpdatePositionDto dto)
    {
        var entity = await _context.Positions.FindAsync(dto.Id);
        if (entity== null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var update = PositionMapper.UpdateToEntity(dto,entity);
        
        _context.Positions.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeletePosition(List<int> positionIds)
    {
        foreach (var id in positionIds)
        {
            var entity = await _context.Positions.Where(x=>positionIds.Contains(x.Id)).ToListAsync();
            _context.Positions.RemoveRange(entity);
            await _context.SaveChangesAsync();
        }
        return Result.Success();
    }
    
    // 

    public async Task<List<GetPositionProjectTagDto>> GetPositionProjectTag(int positionId)
    {
        var entities = await _context.PositionProjectTags.Where(x=>x.PositionId==positionId).ToListAsync();
           return entities
            .Select(x=>PositionProjectTagMapper.GetPositionProjectTag(x))
            .ToList();
    }

    public async Task<Result> CreatePositionProjectTag(CreatePositionProjectTagDto dto)
    {
        var entity = PositionProjectTagMapper.CreatePositionTag(dto);
        _context.PositionProjectTags.Add(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeletePositionProjectTag(List<int> positionIds)
    {
        var entity = await _context.PositionProjectTags.Where(x=>positionIds.Contains(x.Id)).ToListAsync();
         _context.PositionProjectTags.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
    //
    public async Task<List<GetPositionAttributeDto>> GetPositionAttribute(int positionId)
    {
        var entity = await _context.PositionAttributes.Where(x => x.AttributeId == positionId).ToListAsync();
        
        return entity.Select(x=>PositionAttributeMapper.GetPositionAttribute(x)).ToList();
    }

    public async Task<Result> CreatePositionAttribute(CreatePositionAttributeDto dto)
    {
        var entity = PositionAttributeMapper.CreatePositionAttributeDto(dto);
        _context.PositionAttributes.Add(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeletePositionAttribute(List<int> positionIds)
    {
        var entity = await _context.PositionAttributes.Where(x => positionIds.Contains(x.Id)).ToListAsync();
        _context.PositionAttributes.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public List<Operator> GetOperators()
    {
        return System.Enum.GetValues<Operator>().ToList();
    }

    public async Task<Result> CreatePositionAccessRule(CreatePositionAccessRuleDto dto)
    {
        var entity = PositionAccessRuleMapper.CreateAccessRule(dto);
        _context.AccessRules.Add(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<GetPositionAccessRuleDto>> GetPositionAccessRules(int positionId)
    {
        var entity = await _context.AccessRules.Where(x => x.PositionId== positionId).ToListAsync<AccessRule>();
        return entity.Select(x=>PositionAccessRuleMapper.GetPositionAccessRule(x)).ToList();
    }
}