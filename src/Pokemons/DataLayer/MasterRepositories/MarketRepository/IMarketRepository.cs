using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.MarketRepository;

public interface IMarketRepository
{
    Task<Market> CreateMarket(long playerId);
    Task<Market> GetMarketByPlayerId(long playerId);
    Task Save(long playerId);
    Task Update(Market market);
}