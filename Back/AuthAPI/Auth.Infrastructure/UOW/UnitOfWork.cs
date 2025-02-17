﻿using Auth.Core.Abstractions.UOW;
using Auth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auth.Infrastructure.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AuthContext context)
        => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync()
        => _transaction = await _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null) await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}