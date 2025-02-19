using Auth.Application.Exceptions;
using Auth.Application.Validators.Create;
using Auth.Application.Validators.Update;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Abstractions.Services;
using Auth.Core.Abstractions.UOW;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;
using Auth.Core.Entities;
using AutoMapper;

namespace Auth.Application.Services;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly CreateRoleValidator _createRoleValidator;
    private readonly UpdateRoleValidator _updateRoleValidator;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IMapper mapper, CreateRoleValidator createRoleValidator,
        UpdateRoleValidator updateRoleValidator, IRoleRepository roleRepository,
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _createRoleValidator = createRoleValidator;
        _updateRoleValidator = updateRoleValidator;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<RoleDto?> GetRoleByIdAsync(Guid id)
    {
        var role = await _roleRepository.GetByIdAsync(id); 
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto?> GetRoleByNameAsync(string name)
    {
        var role = (await _roleRepository.FindAsync(r => r.Name == name))
            .FirstOrDefault();
        
        return _mapper.Map<RoleDto>(role);
    }

    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto roleDto)
    {
        if (await ExistsByNameAsync(roleDto.Name))
            throw new AuthException(ExceptionType.CredentialsAlreadyExists, "RoleAlreadyExists");
        
        var validator = await _createRoleValidator.ValidateAsync(roleDto);
        if (!validator.IsValid)
            throw new AuthException(ExceptionType.InvalidRequest, 
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<RoleDto>(role);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto roleDto)
    {
       var existingRole = await _roleRepository.GetByIdAsync(id);
       
       var validator = await _updateRoleValidator.ValidateAsync(roleDto);
       if (!validator.IsValid)
           throw new AuthException(ExceptionType.InvalidRequest, 
               string.Join(", ", validator.Errors));
       
       _mapper.Map(roleDto, existingRole);
       await _roleRepository.UpdateAsync(new[] { existingRole });
       await _unitOfWork.SaveChangesAsync();
       
       return _mapper.Map<RoleDto>(existingRole);
    }

    public async Task<bool> DeleteRoleAsync(Guid id)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await _roleRepository.DeleteAsync(id);
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> ExistsByNameAsync(string name)
        => await _roleRepository.AnyAsync(r => r.Name == name);

    public async Task<IEnumerable<UserDto>> GetUsersByRoleIdAsync(Guid roleId)
    {
        var users = await _userRepository.FindAsync(u => u.RoleId == roleId);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<int> GetTotalRolesCountAsync()
        => await _roleRepository.CountAsync();

    public async Task<int> GetUsersCountByRoleAsync(Guid roleId)
    {
        var users = await _userRepository.FindAsync(u => u.RoleId == roleId);
        return users.Count();
    }

    public async Task<IEnumerable<RoleDto>> GetRolesPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new AuthException(ExceptionType.BadRequest, "PaginationError");

        var role = await _roleRepository.GetAllAsync();
        var pagedRoles = role
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        return _mapper.Map<IEnumerable<RoleDto>>(pagedRoles);
    }
}