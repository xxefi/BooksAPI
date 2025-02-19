using Auth.Core.Dtos.Auth;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;
using Auth.Core.Entities;

namespace Auth.Core.Abstractions.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<UserCredentialsDto?> GetUserCredentialsByIdAsync(Guid id);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task<UserDto> UpdateUserCredentialsAsync(Guid userId, UpdateUserCredentialsDto updateUserCredentialsDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<Role?> GetUserRoleAsync(Guid userId);
    Task<IEnumerable<UserDto>> GetUsersByRoleAsync(Guid roleId);
    Task<int> GetTotalUsersCountAsync();
    Task<IEnumerable<UserDto>> GetUsersPageAsync(int pageNumber, int pageSize);
    Task<string> GetUserPasswordHashAsync(Guid userId);
    Task UpdateUserPasswordAsync(Guid userId, string newPasswordHash);
}