using Pokemons.Core.Enums.Battles;

namespace Pokemons.API.Dto.Responses;

public class CommitDamageResponseDto
{
    public long Health { get; set; }
    public long RemainingHealth { get; set; }
    public BattleState BattleState { get; set; }
    public BattleEntityType EntityType { get; set; }
    public int RemainingEnergy { get; set; }
}