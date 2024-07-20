namespace Pokemons.API.Dto.Responses;

public class TapperConfigResponseDto
{
    public long Balance { get; set; }
    public long GoldBalance { get; set; }
    public int Coins { get; set; }
    public int DamagePerClick { get; set; }
    public int Energy { get; set; }
    public int CurrentEnergy { get; set; }
    public int SuperChargeRemaining { get; set; }
    public bool IsFirstEntry { get; set; }
    public required CommitDamageResponseDto EntityData { get; set; }
}