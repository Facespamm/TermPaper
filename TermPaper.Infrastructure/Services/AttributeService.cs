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

    public AttributeService(AppDbContext  context,AttributeCreateCategoryDto  attributeCreateCategoryDto)
    {
        _context  = context;
    }
    public async Task<List<AttributeGetDto>> GetAttribute()
    {
        var attributesList = await _context.Attributes.ToListAsync();
        var listDto = new List<AttributeGetDto>();

        foreach (var attribute  in attributesList)
        {
            listDto.Add(AttributeMapper.GetToDto(attribute ));
        }
        return listDto;
    }

    public async Task<Result> CreateCategory(AttributeCreateCategoryDto dto )
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
        foreach (var entity  in entitys)
        {
            categoryDto.Add(AttributeCategoryMapper.GetDtoToEntity(entity ));
        }
        return categoryDto;
    }

    public async Task<Result> CreateAttribute(CreateAttributeDto dto)
    {
        if (dto==null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        var entity = AttributeMapper.ToEntity(dto);
        await _context.Attributes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
    
    public async Task<Result> AddAttributeValueOption(AttributeValueDto dto)
    {
        if (dto == null) { return Result.Failure(ErrorCode.ValidationFailed); }

        var maxOrder = await _context.AttributeValueOptions
            .Where(x => x.AttributeId == dto.AttributeId)
            .MaxAsync(x=> x.Order) ?? 0;
        AttributeValueOptionMapper.AddToEntity(dto, maxOrder);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> AddToUsersValue(UserAttributesDto dto)
    {
        if (dto== null)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }
        UserAttributeMapper.AddUserAttributes(dto);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}