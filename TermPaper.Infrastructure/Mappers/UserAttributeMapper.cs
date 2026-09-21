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
        return new UserAttributes()
        {
            AttributeId = dto.AttributeId ?? entity.AttributeId,
            Value = dto.Value ?? entity.Value
        };
    }

    public static GetUserAttributeDto GetUserAttribute(UserAttributes entity)
    {
        return new GetUserAttributeDto
        {
            Id = entity.AttributeId,
            AttributeId = entity.AttributeId,
            UserId = entity.UserId,
            Value = entity.Value
        };
    }
}