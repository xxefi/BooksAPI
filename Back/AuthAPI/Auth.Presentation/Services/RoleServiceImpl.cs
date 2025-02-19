using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Update;
using Grpc.Core;
using Role.Grpc;

namespace Auth.Presentation.Services;

public class RoleServiceImpl : RoleService.RoleServiceBase
{
    private readonly IRoleService _roleService;

    public RoleServiceImpl(IRoleService roleService)
    {
        _roleService = roleService;
    }


    public override async Task<RoleListResponse> GetAllRoles(Empty request, ServerCallContext context)
    {
        var roles = await _roleService.GetAllRolesAsync();
        var response = new RoleListResponse();

        foreach (var role in roles)
        {
            response.Roles.Add(new RoleResponse
            {
                Id = role.Id.ToString(),
                Name = role.Name,
                Description = role.Description,
            });
        }

        return response;
    }

    public override async Task<RoleResponse> GetRoleById(RoleRequest request, ServerCallContext context)
    {
        var role = await _roleService.GetRoleByIdAsync(Guid.Parse(request.Id));
        return new RoleResponse
        {
            Id = role.Id.ToString(),
            Name = role.Name,
            Description = role.Description,
        };
    }

    public override async Task<RoleResponse> GetRoleByName(RoleNameRequest request, ServerCallContext context)
    {
        var role = await _roleService.GetRoleByNameAsync(request.Name);
        return new RoleResponse
        {
            Id = role.Id.ToString(),
            Name = role.Name,
            Description = role.Description,
        };
    }

    public override async Task<RoleResponse> CreateRole(CreateRoleRequest request, ServerCallContext context)
    {
        var roleDto = new CreateRoleDto
        {
            Name = request.Name
        };

        var role = await _roleService.CreateRoleAsync(roleDto);
        return new RoleResponse
        {
            Id = role.Id.ToString(),
            Name = role.Name,
            Description = role.Description,
        };
    }

    public override async Task<RoleResponse> UpdateRole(UpdateRoleRequest request, ServerCallContext context)
    {
        var roleDto = new UpdateRoleDto
        {
            Name = request.Name
        };

        var role = await _roleService.UpdateRoleAsync(Guid.Parse(request.Id), roleDto);
        return new RoleResponse
        {
            Id = role.Id.ToString(),
            Name = role.Name,
            Description = role.Description,
        };
    }

    public override async Task<DeleteRoleResponse> DeleteRole(RoleRequest request, ServerCallContext context)
    {
        var result = await _roleService.DeleteRoleAsync(Guid.Parse(request.Id));
        return new DeleteRoleResponse
        {
            Success = result
        };
    }

    public override async Task<UserListResponse> GetUsersByRoleId(RoleRequest request, ServerCallContext context)
    {
        var users = await _roleService.GetUsersByRoleIdAsync(Guid.Parse(request.Id));
        var response = new UserListResponse();

        foreach (var user in users)
        {
            response.Users.Add(new UserDto
            {
                Id = user.Id.ToString(),
                Name = user.Email
            });
        }

        return response;
    }

    public override async Task<RoleListResponse> GetRolesPage(PaginationRequest request, ServerCallContext context)
    {
        var roles = await _roleService.GetRolesPageAsync(request.PageNumber, request.PageSize);
        var response = new RoleListResponse();

        foreach (var role in roles)
        {
            response.Roles.Add(new RoleResponse
            {
                Id = role.Id.ToString(),
                Name = role.Name,
                Description = role.Description,
            });
        }

        return response;
    }
}