using TermPaper.Application.Dto.UsersAtributesDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class UserAttributeMapper
{
    public static UserAttributes AddUserAttributes(UserAttributesAddDto addDto)
    {
        return new UserAttributes()
        {
            AtributeId = addDto.AtributeId,
            UserId = addDto.UserId,
            Value = addDto.Value
        };
    }

    public static UserAttributes UpdateUserAttributes(UpdateUserAttributeDto dto, UserAttributes entity)
    {
        return new UserAttributes()
        {
            AtributeId = dto.AtributeId ?? entity.AtributeId,
            Value = dto.Value ?? entity.Value
        };
    }

    public static GetUserAttributeDto GetUserAttribute(UserAttributes entity)
    {
        return new GetUserAttributeDto
        {
            Id = entity.AtributeId,
            AtributeId = entity.AtributeId,
            UserId = entity.UserId,
            Value = entity.Value
        };
    }
}