using Microsoft.EntityFrameworkCore;
using Pokemons.Core.BackgroundServices.NotificationCreator;
using Pokemons.Core.Enums;
using Pokemons.Core.Enums.Battles;
using Pokemons.DataLayer.Database;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.RatingRepos;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;
using PokemonsDomain.MessageBroker.Models;
using PokemonsDomain.Notification;

namespace Pokemons.DataLayer.MasterRepositories.CommonRepository;

public class CommonRepository : ICommonRepository
{
    public CommonRepository(AppDbContext context, IUnitOfWork unitOfWork, IRatingDatabaseRepository ratingDatabaseRepository)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _ratingDatabaseRepository = ratingDatabaseRepository;
    }
    
    
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRatingDatabaseRepository _ratingDatabaseRepository;

    public async Task CreateUser(CreateUserModel userModel, long playerId)
    {
        await _unitOfWork.BeginTransaction();
        
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

        var missions = await _context.ActiveMissions.Select(a => new Mission
        {
            PlayerId = playerId,
            ActiveMissionId = a.Id
        }).ToListAsync();

        var memberStatus = new MemberGuildStatus
        {
            PlayerId = playerId
        };

        if (userModel.RefId is not null && await _context.Players.AnyAsync(p => p.Id == userModel.RefId))
        {
            var node = new ReferralNode
            {
                ReferralId = playerId,
                ReferrerId = userModel.RefId.Value,
                Inline = 1
            };
            
            await _context.AddAsync(node);
            
            NotificationCreator.AddNotification(new Notification
            {
                PlayerId = userModel.RefId.Value,
                ReferralName = playerId.ToString(),
                NotificationType = NotificationType.Referral
            });
            
            var parent = await _context.ReferralNodes
                .FirstOrDefaultAsync(n => n.ReferralId == node.ReferrerId && n.Inline == 1);
            
            if (parent is not null)
            {
                var secondNode = new ReferralNode
                {
                    ReferralId = playerId,
                    ReferrerId = parent.ReferrerId,
                    Inline = 2
                };

                await _context.AddAsync(secondNode);

                var notification = new Notification
                {
                    PlayerId = secondNode.ReferrerId,
                    ReferralName = playerId.ToString(),
                    NotificationType = NotificationType.Referral
                };
                
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