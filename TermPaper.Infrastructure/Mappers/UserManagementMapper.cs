using TermPaper.Application.Dto;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Mappers;

public class UserManagementMapper
{
    public static UserDto ToDto(AppUser user, List<string> roles, bool isLockedOut)
    {
        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Roles = roles.ToList(),
            EmailConfirmed = user.EmailConfirmed,
            IsLockedOut = isLockedOut
        };
    }
}