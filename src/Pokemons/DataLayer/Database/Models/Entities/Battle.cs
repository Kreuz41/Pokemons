using Pokemons.Core.Enums;
using Pokemons.Core.Enums.Battles;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Battle
{
    public long Id { get; set; }
    public long Health { get; set; }
    public long RemainingHealth { get; set; }
    public BattleState BattleState { get; set; }
    public BattleEntityType EntityType { get; set; }
    public DateTime BattleStartTime { get; set; }
    public DateTime BattleEndTime { get; set; }
    
    public long PlayerId { get; set; }
    public Player Player { get; set; } = null!;
}