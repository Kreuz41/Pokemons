using Microsoft.EntityFrameworkCore;
using Pokemons.Core.BackgroundServices.NotificationCreator;
using Pokemons.Core.Enums;
using Pokemons.Core.Enums.Battles;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.RatingRepos;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;
using PokemonsDomain.MessageBroker.Models;
using PokemonsDomain.Notification;

namespace Pokemons.DataLayer.MasterRepositories.CommonRepository;

public class CommonRepository : ICommonRepository
{
    public CommonRepository(AppDbContext context, IUnitOfWork unitOfWork, 
        IRatingDatabaseRepository ratingDatabaseRepository, IPlayerRepository playerRepository)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _ratingDatabaseRepository = ratingDatabaseRepository;
        _playerRepository = playerRepository;
    }
    
    
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRatingDatabaseRepository _ratingDatabaseRepository;
    private readonly IPlayerRepository _playerRepository;

    public async Task CreateUser(CreateUserModel userModel, long playerId)
    {
        await _unitOfWork.BeginTransaction();

        #region EntitiesCreated
        var player = new Player
        {
            Name = userModel.Name,
            Surname = userModel.Surname,
            PhotoUrl = userModel.PhotoUrl,
            Username = userModel.Username,
            Id = playerId
        };
        player.CurrentEnergy = player.Energy;
        
        var rating = new Rating
        {
            LeagueType = LeagueType.Beginners,
            PlayerId = playerId,
            LeaguePosition = await _ratingDatabaseRepository.GetMaxPositionInLeague(LeagueType.Beginners) + 1,
            GlobalRatingPosition = await _ratingDatabaseRepository.GetMaxPositionInGlobalRating() + 1
        };
        
        var market = new Market
        {
            PlayerId = playerId
        };

        var battle = new Battle
        {
            PlayerId = playerId,
            Health = 1000,
            RemainingHealth = 1000,
            BattleState = BattleState.Battle,
            BattleStartTime = DateTime.UtcNow,
            IsGold = false
        };
        #endregion

        var missions = await _context.ActiveMissions.Select(a => new Mission
        {
            PlayerId = playerId,
            ActiveMissionId = a.Id
        }).ToListAsync();

        var memberStatus = new MemberGuildStatus
        {
            PlayerId = playerId
        };

        var parent1 = await _playerRepository.GetPlayerById(userModel.RefId ?? -1);
        if (userModel.RefId is not null && parent1 is not null)
        {
            var node = new ReferralNode
            {
                ReferralId = playerId,
                ReferrerId = userModel.RefId.Value,
                Inline = 1
            };

            parent1.RefsCount++;
            if (parent1.RefsCount == 4)
                parent1.Balance += 50_000;

            await _playerRepository.Update(player);
            
            await _context.ReferralNodes.AddAsync(node);
            
            NotificationCreator.AddNotification(new Notification
            {
                PlayerId = userModel.RefId.Value,
                ReferralName = playerId.ToString(),
                NotificationType = NotificationType.Referral
            });
            
            var secondRefNode = await _context.ReferralNodes
                .Include(n => n.Referral)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.ReferralId == node.ReferrerId && n.Inline == 1);
            
            if (secondRefNode is not null)
            {
                var secondNode = new ReferralNode
                {
                    ReferralId = playerId,
                    ReferrerId = secondRefNode.ReferrerId,
                    Inline = 2
                };

                await _context.ReferralNodes.AddAsync(secondNode);

                var notification = new Notification
                {
                    PlayerId = secondNode.ReferrerId,
                    ReferralName = playerId.ToString(),
                    NotificationType = NotificationType.Referral
                };

                secondRefNode.Referral.RefsCount++;
                if (secondRefNode.Referral.RefsCount == 4)
                    secondRefNode.Referral.Balance += 50_000;

                await _playerRepository.Update(secondRefNode.Referral);
                
                if (await _context.Notifications.FirstOrDefaultAsync(n => 
                        n.PlayerId == notification.PlayerId
                        && n.ReferralName == notification.ReferralName
                        && n.NotificationType == notification.NotificationType) is null)
                    NotificationCreator.AddNotification(notification);
            }
        }

        await _context.Players.AddAsync(player);
        await _context.Battles.AddAsync(battle);
        await _context.Markets.AddAsync(market);
        await _context.Rating.AddAsync(rating);
        await _context.Missions.AddRangeAsync(missions);
        await _context.MemberGuildStatus.AddAsync(memberStatus);
        await _unitOfWork.CommitTransaction();
    }
}