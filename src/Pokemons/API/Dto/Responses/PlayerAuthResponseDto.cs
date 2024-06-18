namespace Pokemons.API.Dto.Responses;

public class PlayerAuthResponseDto
{
    public int DefeatedEntities { get; set; }
    public int DamagePerClick { get; set; }
    public long Balance { get; set; }
    public int Energy { get; set; }
    public decimal EnergyCharge { get; set; }
    public int SuperCharge { get; set; }
    public decimal SuperChargeCooldown { get; set; }
    public CommitDamageResponseDto EntityData { get; set; } = new();
}