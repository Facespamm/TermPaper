using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Application.Dto;
using TermPaper.Application.Dto.AttributeValueDto;
using TermPaper.Application.Dto.UsersAttributesDto;
using TermPaper.Application.Interface;
using TermPaper.Domain.Models;
using TermPaper.Enum;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Mappers;

namespace TermPaper.Infrastructure.Services;

public class AttributeService:IAttributeService
{
    private readonly AppDbContext _context;

    public AttributeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AttributeGetDto>> GetAttribute()
    {
        var attributesList = await _context.Attributes.ToListAsync();

        return attributesList.Select(AttributeMapper.GetToDto).ToList();
    }

    public async Task<Result> CreateCategory(AttributeCreateCategoryDto dto)
    {
        var entity = AttributeCategoryMapper.ToEntity(dto);
        await _context.AttributeCategories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<AttributeGetCategoryDto>> GetCategory()
    {
        var entitys = await _context.AttributeCategories.ToListAsync();
        return entitys.Select(AttributeCategoryMapper.GetDtoToEntity).ToList();
    }

    public async Task<Result> CreateAttribute(CreateAttributeDto dto)
    {
        var entity =  AttributeMapper.ToEntity(dto);
        await _context.Attributes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddAttributeValueOption(AttributeValueAddDto addDto)
    {
        var maxOrder = await _context.AttributeValueOptions
            .Where(x => x.AttributeId == addDto.AttributeId)
            .MaxAsync(x => x.Order) ?? 0;
        var add = AttributeValueOptionMapper.AddToEntity(addDto, maxOrder);
        await _context.AttributeValueOptions.AddAsync(add);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddToUsersValue(UserAttributesAddDto addDto)
    {
       var entuty = UserAttributeMapper.AddUserAttributes(addDto);
       await _context.UserAttributes.AddAsync(entuty);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAttribute(UpdateAttributeDto dto)
    {
        var entity = await _context.Attributes.FindAsync(dto.Id);
        if (entity == null)
            return Result.Failure(ErrorCode.NotFound);
        
        AttributeMapper.ToUpdateEntity(entity, dto);
        _context.Entry(entity).Property(x=>x.Version).OriginalValue = dto.Version;
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

    public async Task<Result> DeleteAttribute(List<int> attributeIds)
    {
        var entities = await _context.Attributes.Where(x=>attributeIds.Contains(x.Id)).ToListAsync();
         _context.Attributes.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAttributeCategory(AttributeUpdateCategoryDto dto)
    {
        var entity = await _context.AttributeCategories.FindAsync(dto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }
        _context.Entry(entity).Property(x => x.Version).OriginalValue = dto.Version;
        AttributeCategoryMapper.UpdateEntity(dto, entity);
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

    public async Task<Result> DeleteAttributeCategory(List<int> attributeCategoryIds)
    {
        var entities = await _context.AttributeCategories.Where(x=>attributeCategoryIds.Contains(x.Id)).ToListAsync();
        _context.AttributeCategories.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAttributeValue(UpdateAttributeValueDto dto)
    {
        var entity = await _context.AttributeValueOptions.FindAsync(dto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }
        AttributeValueOptionMapper.UpdateToEntity(dto, entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAttributeValueOption(List<int> attributeValueOptionIds)
    {
        var entities = await _context.AttributeValueOptions.Where(x=>attributeValueOptionIds.Contains(x.Id)).ToListAsync();
        _context.AttributeValueOptions.RemoveRange(entities);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateUserAttribute(UpdateUserAttributeDto dto)
    {
        var entity = await _context.UserAttributes.FindAsync(dto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }
        _context.Entry(entity).Property(x => x.Version).OriginalValue = dto.Version;
        UserAttributeMapper.UpdateUserAttributes(dto, entity);
        try
        {
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException e)
        {
            return Result.Failure(ErrorCode.ConcurrencyConflict);
        }
    }

    public async Task<Result> DeleteUserAttribute(List<int> attributeIds)
    {
        var entities = await _context.UserAttributes.Where(x=>attributeIds.Contains(x.Id)).ToListAsync();
        _context.UserAttributes.RemoveRange(entities);
        await _context.SaveChangesAsync();
         return Result.Success();
    }

    public async Task<List<GetUserAttributeDto>> GetOptionValuesAsync(int attributeId)
    {
        var entitys = await _context.UserAttributes.Where(x=>x.AttributeId  == attributeId).ToListAsync();

        return entitys.Select( x=>UserAttributeMapper.GetUserAttribute(x)).ToList();
    } 
    public async Task<List<GetUserAttributeDto>> GetUserAttributeValuesAsync(string userId)
    {
        var entitys = await _context.UserAttributes.Where(x=>x.UserId  == userId).ToListAsync();

        return entitys.Select( x=>UserAttributeMapper.GetUserAttribute(x)).ToList();
    }

    public async Task<List<AttributeGetDto>> SearchByPrefixAsync(string prefix)
    {
        var search = await _context.Attributes.Where(x => x.Name.StartsWith(prefix)).ToListAsync();

        return search.Select(x=>AttributeMapper.GetToDto(x)).ToList();
    }

    public async Task<List<AttributeGetDto>> GetByCategoryAsync(int categoryId)
    {
        var entity = await _context.Attributes.Where(x => x.CategoryId == categoryId).ToListAsync();
        return entity.Select(x=>AttributeMapper.GetToDto(x)).ToList();
    }
    
    public async Task<List<AttributeGetDto>> GetRecentlyUsedAsync(string userId)
    {
        var recently = await _context.RecentlyUsedAttribute.Where(x => x.UserId == userId)
            .Select(y => y.Attribute).Take(10).ToListAsync();
        return recently.Select(x=>AttributeMapper.GetToDto(x)).ToList();
    }

    public async Task<List<AttributeGetDto>> GetBuiltInAttributes()
    {
        await _context.Attributes.Where(a => a.IsBuiltIn).ToListAsync();
        return _context.Attributes.Select(x=>AttributeMapper.GetToDto(x)).ToList();
    }
}