using Pokemons.Core.Enums;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.MarketService;

public interface IMarketService
{
    Task CreateMarket(long playerId);
    Task<Market?> GetMarketByPlayerId(long playerId);
    Task Save(long playerId);
    bool TryUpgradeStat(Player player, Market market, StatType type);
    Task Update(Market market);
}