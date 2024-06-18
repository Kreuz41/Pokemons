using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.MarketRepos;

namespace Pokemons.DataLayer.MasterRepositories.MarketRepository;

public class MarketRepository : IMarketRepository
{
    public MarketRepository(IMarketDatabaseRepository databaseRepository, ICacheRepository cacheRepository)
    {
        _databaseRepository = databaseRepository;
        _cacheRepository = cacheRepository;
    }

    private readonly IMarketDatabaseRepository _databaseRepository;
    private readonly ICacheRepository _cacheRepository;

    public async Task<Market> CreateMarket(long playerId)
    {
        var market = await _databaseRepository.Create(playerId);
        await _cacheRepository.SetMember(playerId.ToString(), market);
        
        return market;
    }

    public async Task Save(long playerId)
    {
        var market = await _cacheRepository.GetMember<Market>(playerId.ToString());
        if (market is null) return;

        await _databaseRepository.Save(market);
        await _cacheRepository.DeleteMember<Market>(playerId.ToString());
    }

    public async Task Update(Market market)
    {
        await _databaseRepository.Save(market);
        await _cacheRepository.SetMember(market.PlayerId.ToString(), market);
    }

    public async Task<Market> GetMarketByPlayerId(long playerId)
    {
        var market = await _cacheRepository.GetMember<Market>(playerId.ToString()) 
                     ?? await _databaseRepository.GetByPlayerId(playerId)
                     ?? throw new NullReferenceException("Market cannot be null");

        await _cacheRepository.SetMember(playerId.ToString(), market);
        
        return market;
    }
}