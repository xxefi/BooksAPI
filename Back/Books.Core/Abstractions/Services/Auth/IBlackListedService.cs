using Books.Core.Dtos.Create;
using Books.Core.Dtos.Read;
using Books.Core.Dtos.Update;
using Books.Core.Models;

namespace Books.Core.Abstractions.Services.Auth;

public interface IBlackListedService
{
    Task<IEnumerable<BlackListedDto>> GetAllBlackListedAsync(); 
    Task<BlackListedDto> GetBlackListedByIdAsync(int id);
    Task<BlackListedDto> AddToBlackListAsync(CreateBlackListedDto createBlackListedDto);
    Task<IEnumerable<BlackListedDto>> UpdateBlackListAsync(IEnumerable<UpdateBlackListedDto> updateBlackListedDto);
    Task<bool> DeleteFromBlackListAsync(int id);
    Task<bool> IsBlackListedAsync(string token);
}