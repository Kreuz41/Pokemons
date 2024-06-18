﻿using Pokemons.Core.Enums.Battles;
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
        var health = 5000 + 5000 * defeatedEntities;
        var entityType = (BattleEntityType)Random.Shared.Next(1, Enum.GetValues<BattleEntityType>().Length + 1);
        var battle = new Battle
        {
            PlayerId = playerId,
            Health = health,
            RemainingHealth = health,
            BattleState = BattleState.Battle,
            BattleStartTime = _timeProvider.Now(),
            EntityType = entityType
        };
        
        await _battleRepository.CreateBattle(battle);
        
        return battle;
    }

    public async Task<Battle?> GetBattleByPlayerId(long playerId) =>
        await _battleRepository.GetPlayerBattle(playerId);
    
    private async Task EndBattle(Battle battle)
    {
        battle.BattleState = BattleState.Defeated;
        battle.BattleEndTime = _timeProvider.Now();
        await _battleRepository.Save(battle);
    }
}