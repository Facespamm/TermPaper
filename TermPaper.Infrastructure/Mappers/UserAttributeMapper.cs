using TermPaper.Application.Dto.UsersAtributesDto;
using TermPaper.Domain.Models;

namespace TermPaper.Infrastructure.Mappers;

public class UserAttributeMapper
{
    public static UserAttributes AddUserAttributes(UserAttributesDto dto)
    {
        return new UserAttributes()
        {
            AtributeId = dto.AtributeId,
            UserId = dto.UserId,
            Value = dto.Value
        };
    }
}