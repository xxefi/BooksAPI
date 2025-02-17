using AutoMapper;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories.Auth;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.UOW;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Update;
using Books.Core.Entities;

namespace Books.Application.Services.Auth;

public class UserActiveSessionsService : IUserActiveSessionsService
{
    private readonly IMapper _mapper;
    private readonly IUserActiveSessionsRepository _userActiveSessionsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserActiveSessionsService(IMapper mapper, IUserActiveSessionsRepository userActiveSessionsRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userActiveSessionsRepository = userActiveSessionsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserActiveSessions> AddUserActiveSessionAsync(CreateUserActiveSessionDto token)
    {
        var userDeviceToken = _mapper.Map<UserActiveSessions>(token);
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _userActiveSessionsRepository.AddAsync(userDeviceToken);
            await _unitOfWork.CommitTransactionAsync();
            return userDeviceToken;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<UserActiveSessions> GetUserActiveSessionAsync(Guid userId)
    {
        var userDeviceToken = (await _userActiveSessionsRepository
            .FindAsync(t => t.UserId == userId)).FirstOrDefault();
        return userDeviceToken;
    }
    
    public async Task<UserActiveSessions> UpdateUserActiveSessionAsync(UpdateUserActiveSessionDto tokenDto)
    {
        var userDeviceToken = await _userActiveSessionsRepository.GetByIdAsync(tokenDto.Id);

        _mapper.Map(tokenDto, userDeviceToken);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _userActiveSessionsRepository.UpdateAsync(new[]{userDeviceToken});
            await _unitOfWork.CommitTransactionAsync();
            return userDeviceToken;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<UserActiveSessions> DeleteUserActiveSessionAsync(Guid tokenId)
    {
        var userDeviceToken = await _userActiveSessionsRepository.GetByIdAsync(tokenId);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _userActiveSessionsRepository.DeleteAsync(tokenId);
            await _unitOfWork.CommitTransactionAsync();
            return userDeviceToken;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> IsUserSessionActiveAsync(Guid userId)
    {
        var activeSessions = await _userActiveSessionsRepository.FindAsync(u => u.UserId == userId );
        return activeSessions.Any();
    }
}