using Pokemons.Core.Services.PlayerService;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;

namespace Pokemons.Core.Services.ReferralService;

public class ReferralService : IReferralService
{
    public ReferralService(IReferralNodeRepository nodeRepository, IPlayerService playerService)
    {
        _nodeRepository = nodeRepository;
        _playerService = playerService;
    }

    private readonly IReferralNodeRepository _nodeRepository;
    private readonly IPlayerService _playerService;

    public async Task CreateNode(long playerId, long referrerId)
    {
        if (!await _playerService.IsPlayerExist(referrerId) || playerId == referrerId) return;
        
        var node = await _nodeRepository.GetReferralNode(playerId);
        if (node is not null) return;

        node = new ReferralNode
        {
            ReferralId = playerId,
            ReferrerId = referrerId,
            Inline = 1
        };
        await _nodeRepository.CreateNode(node);
        
        node = await _nodeRepository.GetReferralNode(referrerId);
        if (node is null) return;
        
        var secondNode = new ReferralNode
        {
            ReferralId = playerId,
            ReferrerId = node.Referrer.Id,
            Inline = 2
        };
        await _nodeRepository.CreateNode(secondNode);
    }

    public async Task<IEnumerable<(Player, int)>> GetReferrals(long playerId) => 
        await _nodeRepository.GetReferrals(playerId);
}