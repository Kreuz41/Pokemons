using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;

namespace Pokemons.Core.Services.ReferralService;

public class ReferralService : IReferralService
{
    public ReferralService(IReferralNodeRepository nodeRepository)
    {
        _nodeRepository = nodeRepository;
    }

    private readonly IReferralNodeRepository _nodeRepository;

    public async Task CreateNode(long playerId, long referrerId)
    {
        var node = await _nodeRepository.GetReferralNode(playerId);
        if (node is not null) return;

        node = new ReferralNode
        {
            ReferralId = playerId,
            ReferrerId = referrerId
        };
        await _nodeRepository.CreateNode(node);
    }

    public async Task<IEnumerable<Player>> GetReferrals(long playerId) => 
        await _nodeRepository.GetReferrals(playerId);
}