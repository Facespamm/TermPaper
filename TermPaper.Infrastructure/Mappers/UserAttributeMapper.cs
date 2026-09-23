using TermPaper.Application.Dto.UsersAttributesDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class UserAttributeMapper
{
    public static UserAttributes AddUserAttributes(UserAttributesAddDto addDto)
    {
        return new UserAttributes()
        {
            AttributeId = addDto.AttributeId,
            UserId = addDto.UserId,
            Value = addDto.Value
            
        };
    }

    public static UserAttributes UpdateUserAttributes(UpdateUserAttributeDto dto, UserAttributes entity)
    {
        entity.AttributeId = dto.AttributeId ??  entity.AttributeId;
        entity.Value = dto.Value ??  entity.Value;
        return entity;
    }

    public static GetUserAttributeDto GetUserAttribute(UserAttributes entity)
    {
        return new GetUserAttributeDto
        {
            Id = entity.Id,
            AttributeId = entity.AttributeId,
            UserId = entity.UserId,
            Value = entity.Value,
            Version = entity.Version, 
            Attribute = AttributeMapper.GetToDto(entity.Attributes)
        };
    }
}