using Auth.Core.Abstractions.Services;
using Auth.Core.Dtos.Create;
using Auth.Core.Dtos.Read;
using Auth.Core.Dtos.Update;

namespace Auth.Application.Services;

public class BlackListedService : IBlackListedService
{
    public async Task<IEnumerable<BlackListedDto>> GetAllBlackListedAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<BlackListedDto> GetBlackListedByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<BlackListedDto> AddToBlackListAsync(CreateBlackListedDto createBlackListedDto)
    {
        throw new NotImplementedException();
    }

    public async Task<BlackListedDto> UpdateBlackListAsync(UpdateBlackListedDto updateBlackListedDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteFromBlackListAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsBlackListedAsync(BlackListedDto blackListedDto)
    {
        throw new NotImplementedException();
    }
}