using System.Linq.Expressions;
using Auth.Application.Exceptions;
using Auth.Core.Abstractions.Repositories;
using Auth.Core.Entities;
using Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;

public class BlackListedRepository : IBlackListedRepository
{
    private readonly AuthContext _context;

    public BlackListedRepository(AuthContext context)
            => _context = context;
    
    public async Task<BlackListed> GetByIdAsync(int id)
        => await _context.BlackListeds
               .AsNoTracking()
               .FirstOrDefaultAsync(b => b.Id == id)
               .ConfigureAwait(false)
           ?? throw new AuthException(ExceptionType.NotFound, "BlackListItemNotFound");

    public async Task<IEnumerable<BlackListed>> GetAllAsync()
    {
        var blackList = await _context.BlackListeds
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);
        
        return blackList.Any() ? blackList : throw new AuthException(ExceptionType.NotFound, "NoBlackListItemsFound");
    }

    public async Task AddAsync(BlackListed blackListed)
        => await _context.BlackListeds.AddAsync(blackListed);

    public async Task UpdateAsync(IEnumerable<BlackListed> blackLists)
    {
        foreach (var blackListed in blackLists)
        {
            var updatedCount = await _context.BlackListeds
                .Where(b => b.Id == blackListed.Id)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(b => b.AccessToken, blackListed.AccessToken)
                    .SetProperty(b => b.RefreshToken, blackListed.RefreshToken)
                    .SetProperty(b => b.IpAddress, blackListed.IpAddress)
                    .SetProperty(b => b.DeviceInfo, blackListed.DeviceInfo));
            
            if (updatedCount == 0) throw new AuthException(ExceptionType.NotFound, "BlackListItemNotFound");
        }
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.BlackListeds
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();
        
        if (item == 0) throw new AuthException(ExceptionType.NotFound, "BlackListItemNotFound");
    }

    public async Task<bool> AnyAsync(Expression<Func<BlackListed, bool>> predicate)
        => await _context.BlackListeds.AnyAsync(predicate);

    public async Task<ICollection<BlackListed>> FindAsync(Expression<Func<BlackListed, bool>> predicate)
        => await _context.BlackListeds
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
}