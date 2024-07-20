namespace Pokemons.API.Dto.Responses;

public class ProfileResponseDto
{
    public int DamagePerClick { get; set; }
    public int DefeatedEntities { get; set; }
    public decimal EnergyCooldown { get; set; }
    public decimal SuperChargeCooldown { get; set; }
    public int TotalDamage { get; set; }
    public int TotalTaps { get; set; }
    public string? PhotoUrl { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public bool IsPremium { get; set; }
    public decimal CryptoBalance { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
}