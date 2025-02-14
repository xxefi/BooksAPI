using AutoMapper;
using Books.Application.Exceptions;
using Books.Core.Abstractions.Repositories;
using Books.Core.Abstractions.Services.Auth;
using Books.Core.Abstractions.UOW;
using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Application.Services.Auth;

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
        var existingItem = await _blackListedRepository.AnyAsync(b => b.Token == createBlackListedDto.Token);
        if (existingItem)
            throw new BookException(ExceptionType.CredentialsAlreadyExists, "TokenAlreadyBlacklisted");
        
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

    public async Task<IEnumerable<BlackListedDto>> UpdateBlackListAsync(IEnumerable<UpdateBlackListedDto> updateBlackListedDto)
    {
        var blackListedItems = new List<BlackListed>();

        foreach (var dto in updateBlackListedDto)
        {
            var existingItem = await _blackListedRepository.GetByIdAsync(dto.Id);
            if (existingItem == null)
                throw new BookException(ExceptionType.NotFound, "BlackListItemNotFound");

            _mapper.Map(dto, existingItem);
            blackListedItems.Add(existingItem);
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _blackListedRepository.UpdateAsync(blackListedItems);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        return _mapper.Map<IEnumerable<BlackListedDto>>(blackListedItems);
    }

    public async Task<bool> DeleteFromBlackListAsync(int id)
    {
        await _blackListedRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> IsBlackListedAsync(string token)
    {
        var blackListedItem = await _blackListedRepository.FindAsync(b => b.Token == token);
        return blackListedItem.Any();
    }
}