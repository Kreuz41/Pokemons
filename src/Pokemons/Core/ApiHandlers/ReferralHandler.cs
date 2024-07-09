using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;
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
    
    public async Task<CallResult<Friends>> GetReferralInfo(long playerId)
    {
        var refs = await _referralService.GetReferrals(playerId);
        var response = refs.Select(value => new FriendsItem
            {
                Id = value.Item1.Id,
                Name = value.Item1.Name,
                Surname = value.Item1.Surname,
                DefeatedEntities = value.Item1.DefeatedEntities,
                Level = value.Item1.Level,
                PhotoUrl = value.Item1.PhotoUrl,
                LeagueType = (int)value.Item1.Rating.LeagueType,
                Inline = value.Item2,
                ProfitNumber = 0
            })
            .ToList();

        return CallResult<Friends>.Success(new Friends
        {
            List = response,
            Total = response.Count
        });
    }
}