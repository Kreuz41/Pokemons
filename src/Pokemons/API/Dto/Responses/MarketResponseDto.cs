namespace Pokemons.API.Dto.Responses;

public class MarketResponseDto
{
    public int DamagePerClickCost { get; set; }
    public int DamagePerClickLevel { get; set; }
    public int DamagePerClickNextValue { get; set; }
    
    public int EnergyCost { get; set; }
    public int EnergyLevel { get; set; }
    public int EnergyNextValue { get; set; }
    
    public int EnergyChargeCost { get; set; }
    public int EnergyChargeLevel { get; set; } 
    public decimal EnergyChargeNextValue { get; set; }
    
    public int SuperChargeCost { get; set; }
    public int SuperChargeLevel { get; set; }
    public int SuperChargeNextValue { get; set; }
    
    public int SuperChargeCooldownCost { get; set; }
    public int SuperChargeCooldownLevel { get; set; }
    public decimal SuperChargeCooldownNextValue { get; set; }
}