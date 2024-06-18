using Pokemons.Core.Enums.Battles;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.BattleService;

public interface IBattleService
{
    Task<Battle> TakeDamage(long playerId, int damage);
    Task<Battle> CreateNewBattle(long playerId, int defeatedEntities);
}