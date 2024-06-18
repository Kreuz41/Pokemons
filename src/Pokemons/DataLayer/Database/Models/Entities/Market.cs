using System.Text.Json.Serialization;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Market
{
    public long Id { get; set; }
    
    public int DamagePerClickCost { get; set; } = 5000;
    public int DamagePerClickLevel { get; set; } = 1;
    
    public int EnergyCost { get; set; } = 5000;
    public int EnergyLevel { get; set; } = 1;
    
    public int EnergyChargeCost { get; set; } = 5000;
    public int EnergyChargeLevel { get; set; } = 1;
    
    public int SuperChargeCost { get; set; } = 5000;
    public int SuperChargeLevel { get; set; }
    
    public int SuperChargeCooldownCost { get; set; } = 5000;
    public int SuperChargeCooldownLevel { get; set; }

    [JsonIgnore] public Player Player { get; set; } = null!;
    public long PlayerId { get; set; }
}