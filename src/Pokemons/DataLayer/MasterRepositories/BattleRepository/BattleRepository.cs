using Pokemons.Core.Enums.Battles;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.BattleRepos;

namespace Pokemons.DataLayer.MasterRepositories.BattleRepository;

public class BattleRepository : IBattleRepository
{
    public BattleRepository(ICacheRepository cacheRepository, IBattleDatabaseRepository databaseRepository, 
        ITimeProvider timeProvider)
    {
        _cacheRepository = cacheRepository;
        _databaseRepository = databaseRepository;
        _timeProvider = timeProvider;
    }

    private readonly ICacheRepository _cacheRepository;
    private readonly IBattleDatabaseRepository _databaseRepository;
    private readonly ITimeProvider _timeProvider;

    public async Task<Battle> TakeDamage(long playerId, long damage)
    {
        var battle = await GetPlayerBattle(playerId) 
                     ?? throw new NullReferenceException("Battle cannot be null");
        
        battle.RemainingHealth -= damage;
        await _cacheRepository.SetMember(playerId.ToString(), battle);

        return battle;
    }

    public async Task<Battle?> GetPlayerBattle(long playerId)
    {
        var battle = await _cacheRepository.GetMember<Battle>(playerId.ToString())
                     ?? await _databaseRepository.GetBattleByPlayerId(playerId);

        if (battle is null) return null;
        
        await _cacheRepository.SetMember(playerId.ToString(), battle);
        
        return battle;
    }

    public async Task<Battle> CreateBattle(Battle battle)
    {
        await _cacheRepository.DeleteMember<Battle>(battle.PlayerId.ToString());
        battle = await _databaseRepository.CreateBattleForPlayer(battle);
        await _cacheRepository.SetMember(battle.PlayerId.ToString(), battle);
        
        return battle;
    }

    public async Task Save(Battle battle)
    {
        await _databaseRepository.UpdateBattle(battle);
        await _cacheRepository.DeleteMember<Battle>(battle.PlayerId.ToString());
    }
}