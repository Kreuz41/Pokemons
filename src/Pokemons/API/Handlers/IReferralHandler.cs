using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.API.Handlers;

public interface IReferralHandler
{
    Task<CallResult.CallResult<IEnumerable<Player>>> GetReferralInfo(long playerId);
}