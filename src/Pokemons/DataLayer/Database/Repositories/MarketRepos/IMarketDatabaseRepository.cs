using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.MarketRepos;

public interface IMarketDatabaseRepository
{
    Task<Market?> GetByPlayerId(long playerId);
    Task<Market> Create(long playerId);
    Task Save(Market market);
}