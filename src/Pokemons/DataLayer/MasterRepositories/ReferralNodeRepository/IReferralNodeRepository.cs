using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;

public interface IReferralNodeRepository
{
    Task CreateNode(ReferralNode node);
    Task<IEnumerable<Player>> GetReferrals(long playerId);
    Task<ReferralNode?> GetReferralNode(long playerId);
}