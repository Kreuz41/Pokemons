using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.ReferralService;

public interface IReferralService
{
    Task CreateNode(long playerId, long referrerId);
    Task<IEnumerable<ReferralInline>> GetReferrals(long playerId);
}