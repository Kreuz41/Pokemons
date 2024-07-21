using Pokemons.Core.Enums.Battles;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.BattleRepos;

namespace Pokemons.DataLayer.MasterRepositories.BattleRepository;

public class BattleRepository : IBattleRepository
{
    private readonly ICacheRepository _cacheRepository;
    private readonly IBattleDatabaseRepository _databaseRepository;

    public BattleRepository(ICacheRepository cacheRepository, IBattleDatabaseRepository databaseRepository)
    {
        _cacheRepository = cacheRepository;
        _databaseRepository = databaseRepository;
    }

    public async Task<Battle> GetPlayerBattle(long playerId)
    {
        var battle = (await _cacheRepository.GetMember<Battle>(playerId.ToString())
                      ?? await _databaseRepository.GetBattleByPlayerId(playerId)) ?? await CreateBattle(new Battle
        {
            PlayerId = playerId,
            EntityTypeId = 1,
            Health = 5000,
            RemainingHealth = 5000,
            BattleState = BattleState.Battle,
            BattleStartTime = DateTime.UtcNow
        });

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

    public async Task FastSave(Battle battle)
    {
        await _cacheRepository.SetMember(battle.PlayerId.ToString(), battle);
    }

    public async Task Save(Battle battle)
    {
        await _databaseRepository.UpdateBattle(battle);
        await _cacheRepository.DeleteMember<Battle>(battle.PlayerId.ToString());
    }
}