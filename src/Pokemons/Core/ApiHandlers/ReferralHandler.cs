using Pokemons.API.CallResult;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.ReferralService;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.ApiHandlers;

public class ReferralHandler : IReferralHandler
{
    public ReferralHandler(IReferralService referralService)
    {
        _referralService = referralService;
    }

    private readonly IReferralService _referralService;
    
    public async Task<CallResult<IEnumerable<Player>>> GetReferralInfo(long playerId)
    {
        var refs = await _referralService.GetReferrals(playerId);
        return CallResult<IEnumerable<Player>>.Success(refs);
    }
}