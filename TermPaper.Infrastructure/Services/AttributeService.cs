using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Application.Dto;
using TermPaper.Application.Dto.AttributeValueDto;
using TermPaper.Application.Dto.UsersAtributesDto;
using TermPaper.Domain.Models;
using TermPaper.Enum;
using TermPaper.Infrastructure.Context;
using TermPaper.Infrastructure.Mappers;

namespace TermPaper.Infrastructure.Services;

public class AttributeService
{
    private readonly AppDbContext _context;

    public AttributeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AttributeGetDto>> GetAttribute()
    {
        var attributesList = await _context.Attributes.ToListAsync();
        var listDto = new List<AttributeGetDto>();

        foreach (var attribute in attributesList)
        {
            listDto.Add(AttributeMapper.GetToDto(attribute));
        }
        return listDto;
    }

    public async Task<Result> CreateCategory(AttributeCreateCategoryDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
        {
            Result.Failure(ErrorCode.ValidationFailed);
        }
        var entity = AttributeCategoryMapper.ToEntity(dto);
        await _context.AttributeCategories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<List<AttributeGetCategoryDto>> GetCategory()
    {
        var entitys = await _context.AttributeCategories.ToListAsync();
        var categoryDto = new List<AttributeGetCategoryDto>();
        foreach (var entity in entitys)
        {
            categoryDto.Add(AttributeCategoryMapper.GetDtoToEntity(entity));
        }
        return categoryDto;
    }

    public async Task<Result> CreateAttribute(CreateAttributeDto dto)
    {
        if (dto == null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var entity = AttributeMapper.ToEntity(dto);
        await _context.Attributes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddAttributeValueOption(AttributeValueAddDto addDto)
    {
        if (addDto == null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
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
        if (addDto == null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        UserAttributeMapper.AddUserAttributes(addDto);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAttribute(UpdateAttributeDto dto)
    {
        var entity = await _context.Attributes.FindAsync(dto.Id);
        if (entity == null)
            return Result.Failure(ErrorCode.ValidationFailed);
        var update = AttributeMapper.ToUpdateEntity(entity, dto);
        _context.Attributes.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAttribute(List<int> attributeIds)
    {
        foreach (var id in attributeIds)
        {
            var entity = await _context.Attributes.FindAsync(id);
            if (entity == null)
            {
                return Result.Failure(ErrorCode.ValidationFailed);
            }
            _context.Attributes.Remove(entity);
        }
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAttributeCategory(AttributeUpdateCategoryDto dto)
    {
        var entity = await _context.AttributeCategories.FindAsync(dto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var update = AttributeCategoryMapper.UpdateEntity(dto, entity);
        _context.AttributeCategories.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAttributeCategory(List<int> attributeCategoryIds)
    {
        foreach (var id in attributeCategoryIds)
        {
            var entity = await _context.AttributeCategories.FindAsync(id);
            if (entity == null)
            {
                return Result.Failure(ErrorCode.ValidationFailed);
            }
            _context.AttributeCategories.Remove(entity);
        }
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAttributeValue(UpdateAttributeValueDto dto)
    {
        var entity = await _context.AttributeValueOptions.FindAsync(dto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var update = AttributeValueOptionMapper.UpdateToEntity(dto, entity);
        _context.AttributeValueOptions.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAttributeValueOption(List<int> attributeValueOptionIds)
    {
        foreach (var id in attributeValueOptionIds)
        {
            var entity = await _context.AttributeValueOptions.FindAsync(id);
            if (entity == null)
            {
                return Result.Failure(ErrorCode.ValidationFailed);
            }
            _context.AttributeValueOptions.Remove(entity);
        }
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateUserAttribute(UpdateUserAttributeDto dto)
    {
        var entity = await _context.UserAttributes.FindAsync(dto.Id);
        if (entity == null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var update = UserAttributeMapper.UpdateUserAttributes(dto, entity);
        _context.UserAttributes.Update(update);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteUserAttribute(List<int> attributeIds)
    {
        foreach (var id in attributeIds)
        {
            var entity = await _context.UserAttributes.FindAsync(id);
            if (entity == null)
            {
                return Result.Failure(ErrorCode.ValidationFailed);
            }
            _context.UserAttributes.Remove(entity);
        }
        await _context.SaveChangesAsync();
         return Result.Success();
    }

    public async Task<List<GetUserAttributeDto>> GetOptionValuesAsync(int attributeId)
    {
        var entitys = await _context.UserAttributes.Where(x=>x.AtributeId  == attributeId).ToListAsync();
        List<GetUserAttributeDto> userDto = new();
        foreach (var entity in entitys)
        {
            userDto.Add(UserAttributeMapper.GetUserAttribute(entity));
        }
        return userDto;
    }

    public async Task<List<AttributeGetDto>> SearchByPrefixAsync(string prefix)
    {
        var search = await _context.Attributes.Where(x => x.Name.StartsWith(prefix)).ToListAsync();
        var listDto = new List<AttributeGetDto>();

        foreach (var attribute in search)
        {
            listDto.Add(AttributeMapper.GetToDto(attribute));
        }
        return listDto;
    }

    public async Task<List<AttributeGetDto>> GetByCategoryAsync(int categoryId)
    {
        var entity = await _context.Attributes.Where(x => x.CategoryId == categoryId).ToListAsync();
        var listDto = new List<AttributeGetDto>();
        foreach (var attribute in entity)
        {
            listDto.Add(AttributeMapper.GetToDto(attribute));
        }
        return listDto;
    }

    public async Task<List<AttributeGetDto>> GetRecentlyUsedAsync(string userId)
    {
        var recently = await _context.RecentlyUsedAttribute.Where(x => x.UserId == userId)
            .Select(y => y.Attribute).Take(10).ToListAsync();
        var listDto = new List<AttributeGetDto>();
        foreach (var attribute in recently)
        {
            listDto.Add(AttributeMapper.GetToDto(attribute));
        }
        return listDto;
    }
}