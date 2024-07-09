using Pokemons.API.Dto.Responses;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.API.Handlers;

public interface IReferralHandler
{
    Task<CallResult.CallResult<Friends>> GetReferralInfo(long playerId);
}