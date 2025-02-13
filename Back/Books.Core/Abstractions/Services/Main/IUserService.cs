using Books.Core.Dtos.Auth;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Main;

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