using Pokemons.Core.Enums.Battles;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.BattleRepos;

public interface IBattleDatabaseRepository
{
    Task<Battle?> GetActiveBattleByPlayerId(long playerId);
    Task<Battle?> GetEndedBattleByPlayerId(long playerId);
    Task<Battle> CreateBattleForPlayer(Battle battle);
    Task UpdateBattle(Battle battle);
}