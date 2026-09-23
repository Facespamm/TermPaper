using TermPaper.Application.Dto;
using TermPaper.Application.Dto.UsersAttributesDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public  class AttributeMapper
{
    public static AttributeGetDto GetToDto(Attributes attribute)
    {
        return new AttributeGetDto
        {
            Id = attribute.Id,
            CategoryId = attribute.CategoryId,
            Name = attribute.Name,
            DataType = attribute.DataType,
            Description = attribute.Description,
            Version =  attribute.Version,
            IsBuiltIn = attribute.IsBuiltIn,
            AttributeValueOptions = attribute.AttributeValueOptions
                .Select(x => AttributeValueOptionMapper.GetValueOption(x))
                .ToList()
        };
    }

    public static Attributes ToEntity(CreateAttributeDto dto)
    {
        return new Attributes
        {
            Name = dto.Name,
            DataType = dto.DataType,
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            IsBuiltIn = false,
        };
    }
    
        public static Attributes ToUpdateEntity(Attributes attribute,UpdateAttributeDto dto)
        {
           attribute.CategoryId = dto.CategoryId ?? attribute.CategoryId;
           attribute.Name = dto.Name ?? attribute.Name;
           attribute.Description = dto.Description ?? attribute.Description;
           return attribute;
        }
        
        public static GetUserAttributeDto ToMeSectionDto(Attributes attribute, string userId)
        {
            var existingValue = attribute.UserAttributes
                .FirstOrDefault(x => x.UserId == userId);

            return new GetUserAttributeDto
            {
                Id = existingValue?.Id ?? 0,
                AttributeId = attribute.Id,
                UserId = userId,
                Value = existingValue?.Value ?? "",
                Version = existingValue?.Version ?? 0,
                Attribute = AttributeMapper.GetToDto(attribute)
            };
        }
        
}