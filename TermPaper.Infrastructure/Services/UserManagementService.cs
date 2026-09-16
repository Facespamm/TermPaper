using TermPaper.Application.Dto;
using TermPaper.Application.Interface;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Services;

public class UserManagementService:IUserManagementService
{
    
    public async Task<List<UserDto>> GetUsers()
    {
        return await Task.FromResult(new List<UserDto>());
    }
}