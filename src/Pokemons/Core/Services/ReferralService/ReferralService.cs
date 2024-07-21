﻿using Pokemons.Core.BackgroundServices.NotificationCreator;
using Pokemons.Core.Services.PlayerService;
using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;
using PokemonsDomain.Notification;

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
        
        NotificationCreator.AddNotification(new Notification
        {
            PlayerId = referrerId,
            ReferralName = playerId.ToString(),
            NotificationType = NotificationType.Referral
        });
        
        node = await _nodeRepository.GetReferralNode(referrerId);
        if (node is null) return;
        
        var secondNode = new ReferralNode
        {
            ReferralId = playerId,
            ReferrerId = node.Referrer.Id,
            Inline = 2
        };
        await _nodeRepository.CreateNode(secondNode);

        NotificationCreator.AddNotification(new Notification
        {
            PlayerId = secondNode.ReferrerId,
            ReferralName = playerId.ToString(),
            NotificationType = NotificationType.Referral
        });
    }

    public async Task<IEnumerable<ReferralInline>> GetReferrals(long playerId) => 
        await _nodeRepository.GetReferrals(playerId);
    
}