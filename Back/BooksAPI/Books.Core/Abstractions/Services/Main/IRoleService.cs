using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;

namespace Books.Core.Abstractions.Services.Main;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(Guid id);
    Task<RoleDto?> GetRoleByNameAsync(string name);
    Task<RoleDto> CreateRoleAsync(CreateRoleDto roleDto);
    Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto roleDto);
    Task<bool> DeleteRoleAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
    Task<IEnumerable<UserDto>> GetUsersByRoleIdAsync(Guid roleId);
    Task<int> GetTotalRolesCountAsync();
    Task<int> GetUsersCountByRoleAsync(Guid roleId);
    Task<IEnumerable<RoleDto>> GetRolesPageAsync(int pageNumber, int pageSize);
}