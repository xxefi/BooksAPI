using Auth.Application.Exceptions;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Abstractions.Services;
using Auth.Core.Abstractions.UOW;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;
using Auth.Core.Entities;
using AutoMapper;

namespace Auth.Application.Services;

public class BlackListedService : IBlackListedService
{
    private readonly IMapper _mapper;
    private readonly IBlackListedRepository _blackListedRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BlackListedService(IMapper mapper, IBlackListedRepository blackListedRepository, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _blackListedRepository = blackListedRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<BlackListedDto>> GetAllBlackListedAsync()
    {
        var blackListedItems = await _blackListedRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<BlackListedDto>>(blackListedItems);
    }

    public async Task<BlackListedDto> GetBlackListedByIdAsync(int id)
    {
        var blackListedItem = await _blackListedRepository.GetByIdAsync(id);
        return _mapper.Map<BlackListedDto>(blackListedItem);
    }

    public async Task<BlackListedDto> AddToBlackListAsync(CreateBlackListedDto createBlackListedDto)
    {
        var existingItem = await _blackListedRepository.AnyAsync(b => b.AccessToken == createBlackListedDto.AccessToken
        && b.RefreshToken == createBlackListedDto.RefreshToken);
        if (existingItem)
            throw new AuthException(ExceptionType.CredentialsAlreadyExists, "TokenAlreadyBlacklisted");
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var blackListed = _mapper.Map<BlackListed>(createBlackListedDto);
            await _blackListedRepository.AddAsync(blackListed);
            await _unitOfWork.CommitTransactionAsync();
            
            return _mapper.Map<BlackListedDto>(blackListed);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<BlackListedDto> UpdateBlackListAsync(UpdateBlackListedDto updateBlackListedDto)
    {
        var existingBlackListed = await _blackListedRepository.GetByIdAsync(updateBlackListedDto.Id);
        
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            _mapper.Map(updateBlackListedDto, existingBlackListed);
            await _blackListedRepository.UpdateAsync(new[] {existingBlackListed});
            await _unitOfWork.CommitTransactionAsync();
            
            return _mapper.Map<BlackListedDto>(existingBlackListed);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteFromBlackListAsync(int id)
    {
        await _blackListedRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> IsBlackListedAsync(BlackListedDto blackListedDto)
    {
        var isAccessTokenBlackListed = await _blackListedRepository.AnyAsync(b => b.AccessToken == blackListedDto.AccessToken);
        var isRefreshTokenBlackListed = await _blackListedRepository.AnyAsync(b => b.RefreshToken == blackListedDto.RefreshToken);
    
        return isAccessTokenBlackListed  || isRefreshTokenBlackListed ;
    }
}