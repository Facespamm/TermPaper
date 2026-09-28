using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Application.Dto.AttributeDto;
using TermPaper.Application.Dto.AttributesCategoryDto;
using TermPaper.Application.Dto.AttributesValueDto;
using TermPaper.Application.Dto.UsersAttributesDto;
using TermPaper.Application.Interface;
using TermPaper.Domain.Enum;
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

        return attributesList.Select(AttributeMapper.ToDto).ToList();
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
        return entitys.Select(AttributeCategoryMapper.ToDto).ToList();
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
        var add = AttributeValueOptionMapper.ToEntity(addDto, maxOrder);
        await _context.AttributeValueOptions.AddAsync(add);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddToUsersValue(UserAttributesAddDto addDto)
    {
       var entity = UserAttributeMapper.ToEntity(addDto);
       await _context.UserAttributes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAttribute(UpdateAttributeDto dto)
    {
        var entity = await _context.Attributes.FindAsync(dto.Id);
        if (entity == null)
            return Result.Failure(ErrorCode.NotFound);
        _context.Entry(entity).Property(x => x.Version).OriginalValue = dto.Version;  
        AttributeMapper.UpdateEntity(dto, entity);                                    
        return await _context.SaveWithConcurrencyAsync();
    }    public async Task<Result> DeleteAttribute(List<int> attributeIds)
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
       return await _context.SaveWithConcurrencyAsync();
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
        AttributeValueOptionMapper.UpdateEntity(dto, entity);
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

    public async Task<Result> UpdateUserAttribute(string userId,UpdateUserAttributeDto dto)
    {
        var entity = await _context.UserAttributes.FindAsync(dto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.NotFound);
        }
        if (entity.UserId != userId)
        {
            return Result.Failure(ErrorCode.AccessDenied);
        }

        _context.Entry(entity).Property(x => x.Version).OriginalValue = dto.Version;
        UserAttributeMapper.UpdateEntity(dto, entity);
        return await _context.SaveWithConcurrencyAsync();
    }

    public async Task<Result> DeleteUserAttribute(List<int> attributeIds,string userId)
    {
        var entities = await _context.UserAttributes.Where(x=>attributeIds.Contains(x.Id) && x.UserId == userId).ToListAsync();
        _context.UserAttributes.RemoveRange(entities);
        await _context.SaveChangesAsync();
         return Result.Success();
    }

    public async Task<List<AttributeValueOptionDto>> GetOptionValuesAsync(int attributeId)
    {
        var options = await _context.AttributeValueOptions
            .Where(x => x.AttributeId == attributeId)
            .OrderBy(x => x.Order)
            .ToListAsync();
        return options.Select(x=>AttributeValueOptionMapper.ToDto(x)).ToList();
    }   
    public async Task<List<GetUserAttributeDto>> GetUserAttributeValuesAsync(string userId)
    {
        var entitys = await _context.UserAttributes
            .Where(x=>x.UserId  == userId)
            .Include(x=>x.Attributes)
            .ThenInclude(x=>x.AttributeValueOptions)
            .ToListAsync();

        return entitys.Select( x=>UserAttributeMapper.ToDto(x)).ToList();
    }

    public async Task<List<AttributeGetDto>> SearchByPrefixAsync(string prefix)
    {
        var search = await _context.Attributes
            .Where(x => EF.Functions.ILike(x.Name, prefix + "%"))
            .ToListAsync();
        return search.Select(x => AttributeMapper.ToDto(x)).ToList();
    }
          
    public async Task<List<AttributeGetDto>> GetByCategoryAsync(int categoryId)
    {
        var entity = await _context.Attributes.Where(x => x.CategoryId == categoryId).ToListAsync();
        return entity.Select(x=>AttributeMapper.ToDto(x)).ToList();
    }
    
        public async Task<List<GetUserAttributeDto>> GetUserBuiltInAttributeValuesAsync(string userId)
        {
            var attributes = await _context.Attributes
                .Include(a => a.AttributeValueOptions)
                .Include(a => a.UserAttributes.Where(v => v.UserId == userId))   
                .Where(a => a.IsBuiltIn)
                .ToListAsync();

            return attributes.Select(attr => AttributeMapper.ToUserAttributeDto(attr, userId)).ToList();
        }
    
    public async Task<List<AttributeGetDto>> GetRecentlyUsedAsync(string userId)
    {
        var recently = await _context.RecentlyUsedAttribute.Where(x => x.UserId == userId)
            .Select(y => y.Attribute).Take(10).ToListAsync();
        return recently.Select(x=>AttributeMapper.ToDto(x)).ToList();
    }

    public async Task<List<AttributeGetDto>> GetBuiltInAttributes()
    {
        var attributes = await _context.Attributes
            .Where(a => a.IsBuiltIn)
            .ToListAsync();
        return attributes
            .Select(x => AttributeMapper.ToDto(x))
            .ToList();    }
}