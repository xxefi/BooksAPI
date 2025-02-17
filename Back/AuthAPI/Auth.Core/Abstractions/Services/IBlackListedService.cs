using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;

namespace Auth.Core.Abstractions.Services;

public interface IBlackListedService
{
    Task<IEnumerable<BlackListedDto>> GetAllBlackListedAsync(); 
    Task<BlackListedDto> GetBlackListedByIdAsync(int id);
    Task<BlackListedDto> AddToBlackListAsync(CreateBlackListedDto createBlackListedDto);
    Task<BlackListedDto> UpdateBlackListAsync(UpdateBlackListedDto updateBlackListedDto);
    Task<bool> DeleteFromBlackListAsync(int id);
    Task<bool> IsBlackListedAsync(BlackListedDto blackListedDto); 
}