using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.ReferralRepos;

public interface IReferralNodeDatabaseRepository
{
    Task CreateNode(ReferralNode node);
    Task<IEnumerable<ReferralInline>> GetReferrals(long playerId);
    Task<ReferralNode?> GetFirstReferralNode(long referral);
}