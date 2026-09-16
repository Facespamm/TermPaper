using TermPaper.Application.Common;
using TermPaper.Application.Dto;

namespace TermPaper.Application.Interface;

public interface IUserManagementService
{   
    Task<List<UserDto>> GetUsersAsync();
    Task<Result> DeleteUsersAsync(List<string> users);
    Task<Result> DeleteUserRolesAsync(string userId, List<string> roles);
    Task<Result> AddUserRolesAsync(string userId, List<string> roles);
    
}