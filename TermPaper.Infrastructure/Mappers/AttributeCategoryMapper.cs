using TermPaper.Application.Dto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class AttributeCategoryMapper
{
    public static AttributeCategory ToEntity(AttributeCreateCategoryDto dto)
    {
        return new AttributeCategory
        {
            Name = dto.Name,
        };
    }
    
    public static AttributeGetCategoryDto GetDtoToEntity(AttributeCategory entity)
    {
        return new AttributeGetCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
    }

    public static AttributeCategory UpdateEntity(AttributeUpdateCategoryDto dto,AttributeCategory entity)
    {
       entity.Name = dto.Name ?? entity.Name;
       return entity;
    }
}