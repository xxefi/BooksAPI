using Auth.Application.Exceptions;
using Auth.Application.Validators.Create;
using Auth.Application.Validators.Update;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Abstractions.Services;
using Auth.Core.Abstractions.UOW;
using Auth.Core.Dtos.Auth;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;
using Auth.Core.Entities;
using AutoMapper;
using static BCrypt.Net.BCrypt;

namespace Auth.Application.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly CreateUserValidator _createUserValidator;
    private readonly UpdateUserValidator _updateUserValidator;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IMapper mapper, CreateUserValidator createUserValidator,
        UpdateUserValidator updateUserValidator, IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _createUserValidator = createUserValidator;
        _updateUserValidator = updateUserValidator;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
       var user = await _userRepository.GetByIdAsync(id);
       return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = (await _userRepository.FindAsync(u => u.Username == username))
            .FirstOrDefault();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = (await _userRepository.FindAsync(u => u.Email == email))
            .FirstOrDefault();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserCredentialsDto?> GetUserCredentialsByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return new UserCredentialsDto
        {
            Id = user.Id,
            Password = user.Password,
        };
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        if (await ExistsByEmailAsync(createUserDto.Email))
            throw new AuthException(ExceptionType.CredentialsAlreadyExists, "UserAlreadyExists");
        
        var validator = await _createUserValidator.ValidateAsync(createUserDto);
        if (!validator.IsValid)
            throw new AuthException(ExceptionType.InvalidCredentials, 
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var user = _mapper.Map<User>(createUserDto);
            user.Password = HashPassword(createUserDto.Password);
            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitTransactionAsync();
            
            return _mapper.Map<UserDto>(user);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        var validator = await _updateUserValidator.ValidateAsync(updateUserDto);
        if (!validator.IsValid)
            throw new AuthException(ExceptionType.InvalidCredentials, 
                string.Join(", ", validator.Errors));
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            _mapper.Map(updateUserDto, existingUser);
            await _userRepository.UpdateAsync(new[] { existingUser });
            await _unitOfWork.CommitTransactionAsync();
            
            return _mapper.Map<UserDto>(existingUser);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<UserDto> UpdateUserCredentialsAsync(Guid userId, UpdateUserCredentialsDto updateUserCredentialsDto)
    {
        var existingUser = await _userRepository.GetByIdAsync(userId);
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            _mapper.Map(updateUserCredentialsDto, existingUser);
            await _userRepository.UpdateAsync(new[] { existingUser });
            await _unitOfWork.CommitTransactionAsync();
            
            return _mapper.Map<UserDto>(existingUser);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _userRepository.DeleteAsync(id);
            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email)
        => await _userRepository.AnyAsync(u => u.Email == email);

    public async Task<Role?> GetUserRoleAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user?.Role;
    }

    public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(Guid roleId)
    {
        var users = await _userRepository.FindAsync(u => u.RoleId == roleId);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<int> GetTotalUsersCountAsync()
        => await _userRepository.CountAsync();

    public async Task<IEnumerable<UserDto>> GetUsersPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new AuthException(ExceptionType.BadRequest, "PaginationError");

        var users = await _userRepository.GetAllAsync();
        var pagedUsers = users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        return _mapper.Map<IEnumerable<UserDto>>(pagedUsers);
    }

    public async Task<string> GetUserPasswordHashAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        return user.Password;
    }

    public async Task UpdateUserPasswordAsync(Guid userId, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        user.Password = newPassword;
        await _userRepository.UpdateAsync(new[]{user});
    }
}