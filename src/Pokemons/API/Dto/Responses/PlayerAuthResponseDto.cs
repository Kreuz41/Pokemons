namespace Pokemons.API.Dto.Responses;

public class PlayerAuthResponseDto
{
    public int Defeated { get; set; }
    public int DamagePerClick { get; set; }
    public long Balance { get; set; }
    public CommitDamageResponseDto EntityData { get; set; } = new();
}