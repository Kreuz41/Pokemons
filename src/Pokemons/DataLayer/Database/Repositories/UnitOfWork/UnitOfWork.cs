using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace Pokemons.DataLayer.Database.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    private readonly ILogger<UnitOfWork> _logger;

    public async Task BeginTransaction()
    {
        var isTransactionOpen = false;
        while (!isTransactionOpen)
        {
            try
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                isTransactionOpen = true;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                await Task.Delay(100);
                await BeginTransaction();
            }
        }
    }

    public async Task RollbackTransaction()
    {
        if (_transaction is null) return;
        
        await _transaction.RollbackAsync();
        await _transaction.DisposeAsync();
    }

    public async Task CommitTransaction()
    {
        if (_transaction is null) return;

        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }
        catch (NpgsqlException exception) when (exception.SqlState == "53300")
        {
            _logger.LogWarning(exception.Message);
            await Task.Delay(100);
            await CommitTransaction();
        }
        catch (Exception e)
        {
            await RollbackTransaction();
            _logger.LogError("Error while saving changes\nInner: {e}", e);
            throw;
        }
    }
}