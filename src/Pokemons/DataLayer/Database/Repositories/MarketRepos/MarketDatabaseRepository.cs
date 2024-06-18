using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.MarketRepos;

public class MarketDatabaseRepository : IMarketDatabaseRepository
{
    public MarketDatabaseRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Market> GetByPlayerId(long playerId) =>
        await _context.Markets.FirstOrDefaultAsync(p => p.PlayerId == playerId) 
        ?? throw new NullReferenceException("Market cannot be null");

    public async Task UpdateMarket(Market market)
    {
        await _unitOfWork.BeginTransaction();
        _context.Markets.Update(market);
        await _unitOfWork.CommitTransaction();
    }

    public async Task<Market> Create(long playerId)
    {
        var market = new Market
        {
            PlayerId = playerId
        };

        await _unitOfWork.BeginTransaction();
        await _context.Markets.AddAsync(market);
        await _unitOfWork.CommitTransaction();
        
        return market;
    }

    public async Task Save(Market market)
    {
        await _unitOfWork.BeginTransaction();
        var trackedEntity = _context.ChangeTracker.Entries<Market>()
            .FirstOrDefault(e => e.Entity.Id == market.Id);
        if (trackedEntity != null)
            _context.Entry(trackedEntity.Entity).State = EntityState.Detached;
        
        _context.Attach(market);
        _context.Entry(market).State = EntityState.Modified;
        _context.Markets.Update(market);
        await _unitOfWork.CommitTransaction();
    }
}