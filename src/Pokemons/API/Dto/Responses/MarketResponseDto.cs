namespace Pokemons.API.Dto.Responses;

public class MarketResponseDto
{
    public int DamagePerClickCost { get; set; } = 5000;
    public int DamagePerClickLevel { get; set; } = 1;
    public int DamagePerClickNextValue { get; set; } = 2;
    
    public int EnergyCost { get; set; } = 5000;
    public int EnergyLevel { get; set; } = 1;
    public int EnergyNextValue { get; set; } = 10000;
    
    public int EnergyChargeCost { get; set; } = 5000;
    public int EnergyChargeLevel { get; set; } = 1;
    public decimal EnergyChargeNextValue { get; set; } = 1.02m;
    
    public int SuperChargeCost { get; set; } = 5000;
    public int SuperChargeLevel { get; set; }
    public int SuperChargeNextValue { get; set; } = 10000;
    
    public int SuperChargeCooldownCost { get; set; } = 5000;
    public int SuperChargeCooldownLevel { get; set; }
    public decimal SuperChargeCooldownNextValue { get; set; } = 1.02m;
}