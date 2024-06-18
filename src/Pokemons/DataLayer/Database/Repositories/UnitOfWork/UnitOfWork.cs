using Microsoft.EntityFrameworkCore.Storage;

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

    public async Task BeginTransaction() =>
        _transaction = await _context.Database.BeginTransactionAsync();

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
        catch (Exception e)
        {
            await RollbackTransaction();
            _logger.LogError("Error while saving changes\nInner: {e}", e);
            throw;
        }
    }
}