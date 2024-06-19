using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.ReferralService;

public interface IReferralService
{
    Task CreateNode(long playerId, long referrerId);
    Task<IEnumerable<Player>> GetReferrals(long playerId);
}