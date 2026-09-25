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
        var query = _context.Positions.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => 
                (x.Title != null && x.Title.Contains(search)) || 
                (x.ShortDescription != null && x.ShortDescription.Contains(search)));
        }
        var records = await query.ToListAsync();
        return records.Select(x => PositionMapper.GetToPosition(x)).ToList();
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
            return Result.Failure(ErrorCode.NotFound);
        } 
        _context.Entry(entity).Property(x=>x.Version).OriginalValue = dto.Version;
        
        PositionMapper.UpdateToEntity(dto,entity);
        try
        {
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Failure(ErrorCode.ConcurrencyConflict);
        }
    }

    public async Task<Result> DeletePosition(List<int> positionIds)
    {
            var entity = await _context.Positions.Where(x=>positionIds.Contains(x.Id)).ToListAsync();
            _context.Positions.RemoveRange(entity);
            await _context.SaveChangesAsync();
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
        var entity = await _context.PositionProjectTags.Where(x=>positionIds.Contains(x.PositionId)).ToListAsync();
         _context.PositionProjectTags.RemoveRange(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
    //
    public async Task<List<GetPositionAttributeDto>> GetPositionAttribute(int positionId)
    {
        var entity = await _context.PositionAttributes.Where(x => x.PositionId == positionId).ToListAsync();
        
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
        var entity = await _context.PositionAttributes.Where(x => positionIds.Contains(x.PositionId)).ToListAsync();
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
        var entity = await _context.AccessRules.Where(x => x.PositionId== positionId).ToListAsync();
        return entity.Select(x=>PositionAccessRuleMapper.GetPositionAccessRule(x)).ToList();
    }

    public async Task<Result> DeletePositionAccessRule(List<int> positionIds)
    {
        var entities = await _context.AccessRules.Where(x => positionIds.Contains(x.PositionId)).ToListAsync();
        _context.AccessRules.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
    
    private static bool MatchesRule(Operator op, string? userValue, string ruleValue)
    {
        if (userValue == null)
            return false;

        return op switch
        {
            Operator.Equals => userValue == ruleValue,
            Operator.NotEquals => userValue != ruleValue,
            Operator.GreaterThan => decimal.TryParse(userValue, out var uv1) && decimal.TryParse(ruleValue, out var rv1) && uv1 > rv1,
            Operator.LessThan => decimal.TryParse(userValue, out var uv2) && decimal.TryParse(ruleValue, out var rv2) && uv2 < rv2,
            Operator.GreaterThanOrEqual => decimal.TryParse(userValue, out var uv3) && decimal.TryParse(ruleValue, out var rv3) && uv3 >= rv3,
            Operator.LessThanOrEqual => decimal.TryParse(userValue, out var uv4) && decimal.TryParse(ruleValue, out var rv4) && uv4 <= rv4,
            Operator.IsTrue => userValue == "true",
            Operator.Contains => userValue.Contains(ruleValue, StringComparison.OrdinalIgnoreCase),
            _ => false
        };
    }
        public async Task<bool> CheckCandidateAccess(int positionId, string candidateUserId)
    {
        var rules = await _context.AccessRules
            .Where(r => r.PositionId == positionId)
            .ToListAsync();
        if (!rules.Any()) return true;
        var userValues = await _context.UserAttributes
            .Where(ua => ua.UserId == candidateUserId)
            .ToDictionaryAsync(ua => ua.AttributeId, ua => ua.Value);
        return rules.All(r => MatchesRule(r.Operator, userValues.GetValueOrDefault(r.AttributeId), r.Value));
    }
    public async Task<List<GetPositionDto>> GetAvailablePositionsForCandidate(string candidateUserId)
    {
        var positions = await _context.Positions.ToListAsync();
        var rules = await _context.AccessRules.ToListAsync();
        var userValues = await _context.UserAttributes
            .Where(ua => ua.UserId == candidateUserId)
            .ToDictionaryAsync(ua => ua.AttributeId, ua => ua.Value);
        var available = positions.Where(p =>
            rules.Where(r => r.PositionId == p.Id)
                .All(r => MatchesRule(r.Operator, userValues.GetValueOrDefault(r.AttributeId), r.Value)));
        return available.Select(PositionMapper.GetToPosition).ToList();
    }
}