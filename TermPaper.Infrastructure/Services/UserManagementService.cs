using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TermPaper.Application.Common;
using TermPaper.Application.Dto;
using TermPaper.Application.Interface;
using TermPaper.Enum;
using TermPaper.Infrastructure.Mappers;
using TermPaper.Interface;
using TermPaper.Models;

namespace TermPaper.Infrastructure.Services;

public class UserManagementService : IUserManagementService
{
    private readonly UserManager<AppUser> _userManager;

    public UserManagementService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDto>> GetUsersAsync() 
    {
        var users = await _userManager.Users.ToListAsync();
        var usersDto = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user); 
            var isLockedOut = await _userManager.IsLockedOutAsync(user);
            usersDto.Add(UserManagementMapper.ToDto(user, roles, isLockedOut));
        }

        return usersDto;
    }

    public async Task<Result> DeleteUsersAsync(List<string> users)
    {
        foreach (var user in users)
        {
            var userToDelete = await _userManager.FindByIdAsync(user);
            if (userToDelete is null)
            {
                return Result.Failure(ErrorCode.UserNotFound);
            }

            var result = await _userManager.DeleteAsync(userToDelete);
            if (!result.Succeeded)
            {
                return Result.Failure(ErrorCode.ValidationFailed);
            }
        }

        return Result.Success();
    }
    
    public async Task<Result> DeleteUserRolesAsync(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(ErrorCode.UserNotFound);
        }

        var delete = await _userManager.RemoveFromRolesAsync(user, roles);
        if (!delete.Succeeded)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }

        return Result.Success();
    }

    public async Task<Result> AddUserRolesAsync(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(ErrorCode.UserNotFound);
        }

        var add = await _userManager.AddToRolesAsync(user, roles);
        if (!add.Succeeded)
        {
            return Result.Failure(ErrorCode.ValidationFailed);
        }

        return Result.Success();
    }

    public async Task<Result> UnlockUserAsync(List<string> userId)
    {
        foreach (var user in userId)
        {
            var result = await _userManager.FindByIdAsync(user);
            if (result is null)
            { return Result.Failure(ErrorCode.UserNotFound); }
            var unlock = await _userManager.SetLockoutEndDateAsync(result, null);
            if (!unlock.Succeeded)
            { return Result.Failure(ErrorCode.ValidationFailed); }
        }
        return Result.Success();
    }

    public async Task<Result> LockUserAsync(List<string> userId)
    {
        foreach (var user in userId)
        {
            var result = await _userManager.FindByIdAsync(user);
            if (result is null)
            { return Result.Failure(ErrorCode.UserNotFound);}
            var userLock = await _userManager.SetLockoutEndDateAsync(result, DateTimeOffset.MaxValue);
            if (!userLock.Succeeded)
            { return Result.Failure(ErrorCode.ValidationFailed); }
        }
        return  Result.Success();
    }

}