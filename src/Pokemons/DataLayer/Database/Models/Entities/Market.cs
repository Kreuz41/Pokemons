using System.Text.Json.Serialization;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Market
{
    public long Id { get; set; }
    
    public long DamagePerClickCost { get; set; } = 1000;
    public int DamagePerClickLevel { get; set; } = 1;
    public int DamagePerClickNextValue { get; set; } = 2;
    
    public long EnergyCost { get; set; } = 20000;
    public int EnergyLevel { get; set; } = 1;
    public int EnergyNextValue { get; set; } = 1040;
    
    public long EnergyChargeCost { get; set; } = 10000;
    public int EnergyChargeLevel { get; set; } = 1;
    public decimal EnergyChargeNextValue { get; set; } = 3.46m;
    
    public long SuperChargeCost { get; set; } = 5000;
    public int SuperChargeLevel { get; set; }
    public int SuperChargeNextValue { get; set; } = 5760;
    
    public long SuperChargeCooldownCost { get; set; } = 5000;
    public int SuperChargeCooldownLevel { get; set; }
    public decimal SuperChargeCooldownNextValue { get; set; } = 0.25m;

    [JsonIgnore] public Player Player { get; set; } = null!;
    public long PlayerId { get; set; }
}