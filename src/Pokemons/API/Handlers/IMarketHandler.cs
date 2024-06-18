using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;
using Pokemons.Core.Enums;

namespace Pokemons.API.Handlers;

public interface IMarketHandler
{
    Task<CallResult<MarketResponseDto>> GetMarketByUserId(long playerId);
    Task<CallResult<MarketResponseDto>> UpgradeUserStat(long playerId, StatType type);
}   