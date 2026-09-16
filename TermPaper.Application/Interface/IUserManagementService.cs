using TermPaper.Application.Dto;

namespace TermPaper.Application.Interface;

public interface IUserManagementService
{   
    Task<List<UserDto>> GetUsers();
}