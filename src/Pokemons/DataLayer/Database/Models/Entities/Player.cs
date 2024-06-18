using System.Text.Json.Serialization;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Player
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhotoUrl { get; set; }
    public long Balance { get; set; }
    public int DamagePerClick { get; set; } = 1;
    
    public int Energy { get; set; } = 1000;
    public int CurrentEnergy { get; set; } = 1000;
    public DateTime LastCommitDamageTime { get; set; }
    public decimal EnergyCharge { get; set; } = 1;

    public int SuperCharge { get; set; } = 10000;
    public decimal SuperChargeCooldown { get; set; } = 1;
    public DateTime LastSuperChargeActivatedTime { get; set; }
    
    public int Taps { get; set; }
    public int TotalDamage { get; set; }
    public int DefeatedEntities { get; set; }

    public Market Market { get; set; } = null!;
    public long MarketId { get; set; }
    [JsonIgnore] public ICollection<Battle> Battles { get; set; } = new List<Battle>();
}