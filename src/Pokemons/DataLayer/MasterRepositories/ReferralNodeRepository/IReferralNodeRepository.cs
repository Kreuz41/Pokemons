using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;

public interface IReferralNodeRepository
{
    Task CreateNode(ReferralNode node);
    Task<IEnumerable<ReferralInline>> GetReferrals(long playerId);
    Task<ReferralNode?> GetReferralNode(long playerId);
    Task<IEnumerable<ReferralNode>> GetParentsForPlayer(long playerId);
    Task UpdateEnumerable(IEnumerable<ReferralNode> parents);
}