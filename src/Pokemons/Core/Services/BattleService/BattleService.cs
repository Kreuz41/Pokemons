using Pokemons.Core.Enums.Battles;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;

namespace Pokemons.Core.Services.BattleService;

public class BattleService : IBattleService
{
    public BattleService(IBattleRepository battleRepository, ITimeProvider timeProvider)
    {
        _battleRepository = battleRepository;
        _timeProvider = timeProvider;
    }

    private readonly IBattleRepository _battleRepository;
    private readonly ITimeProvider _timeProvider;
    
    private const int EntitiesCount = 20;
    private const int GoldEntitiesCount = 5;

    public async Task<Battle> TakeDamage(long playerId, int damage)
    {
        var battle = await _battleRepository.GetPlayerBattle(playerId);

        battle.RemainingHealth -= damage;
        if (battle.RemainingHealth > 0)
        {
            await _battleRepository.FastSave(battle);
            return battle;
        }
        
        await EndBattle(battle);
        return battle;
    }

    public async Task<Battle> CreateNewBattle(long playerId, int defeatedEntities)
    {
        var health = (int)(500 * Math.Pow(1.015, defeatedEntities));
        var battle = new Battle
        {
            PlayerId = playerId,
            Health = health,
            RemainingHealth = health,
            BattleState = BattleState.Battle,
            BattleStartTime = _timeProvider.Now(),
            IsGold = (defeatedEntities + 1) % 6 == 0,
        };

        battle.EntityTypeId = Random.Shared.Next(battle.IsGold ? GoldEntitiesCount : EntitiesCount);
        
        await _battleRepository.CreateBattle(battle);
        
        return battle;
    }

    public async Task<Battle?> GetBattleByPlayerId(long playerId) =>
        await _battleRepository.GetPlayerBattle(playerId);

    public async Task Save(long playerId)
    {
        var battle = await _battleRepository.GetPlayerBattle(playerId);
        if (battle is not null)
            await _battleRepository.Save(battle);
    }

    private async Task EndBattle(Battle battle)
    {
        battle.BattleState = BattleState.Defeated;
        battle.BattleEndTime = _timeProvider.Now();
        await _battleRepository.Save(battle);
    }
}