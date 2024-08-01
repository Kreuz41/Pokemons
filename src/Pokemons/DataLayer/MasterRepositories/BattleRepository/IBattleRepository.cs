using Pokemons.Core.Enums.Battles;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.BattleRepository;

public interface IBattleRepository
{
    Task<Battle?> GetPlayerBattle(long playerId);
    Task<Battle?> GetLastEndedBattle(long playerId);
    Task<Battle> CreateBattle(Battle battle);
    Task Save(Battle battle);
    Task FastSave(Battle battle);
}