using Pokemons.Core.Enums;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.MarketRepository;

namespace Pokemons.Core.Services.MarketService;

public class MarketService : IMarketService
{
    public MarketService(IMarketRepository marketRepository, ITimeProvider timeProvider)
    {
        _marketRepository = marketRepository;
        _timeProvider = timeProvider;
    }

    private readonly IMarketRepository _marketRepository;
    private readonly ITimeProvider _timeProvider;

    public async Task CreateMarket(long playerId) =>
        await _marketRepository.CreateMarket(playerId);

    public async Task<Market?> GetMarketByPlayerId(long playerId) =>
        await _marketRepository.GetMarketByPlayerId(playerId);

    public async Task Save(long playerId) =>
        await _marketRepository.Save(playerId);
    
    public bool TryUpgradeStat(Player player, Market market, StatType type)
    {
        return type switch
        {
            StatType.SuperChargeDamage => UpgradeSuperChargeDamage(player, market),
            StatType.DamagePerClick => UpgradeDamagePerClick(player, market),
            StatType.EnergyCapacity => UpgradeEnergyCapacity(player, market),
            StatType.EnergyCharge => UpgradeEnergyCharge(player, market),
            StatType.SuperChargeCooldown => UpgradeSuperChargeCooldown(player, market),
            _ => false
        };
    }

    public async Task Update(Market market) =>
        await _marketRepository.Update(market);

    #region UpgradeStats
    private bool UpgradeSuperChargeCooldown(Player player, Market market)
    {
        if (player.GoldBalance < market.SuperChargeCooldownCost || market.SuperChargeCooldownLevel >= 20) return false;

        player.GoldBalance -= market.SuperChargeCooldownCost;
        player.SuperChargeCooldown = market.SuperChargeCooldownNextValue;
        market.SuperChargeCooldownNextValue = (decimal)(8 - 0.2 * market.SuperChargeCooldownLevel);
        market.SuperChargeCooldownCost = (long)Math.Ceiling(400 * Math.Pow(1.75, market.SuperChargeCooldownLevel));
        if (market.SuperChargeCooldownLevel == 0)
            player.LastSuperChargeActivatedTime = _timeProvider.Now();
        
        market.SuperChargeCooldownLevel++;
        
        
        return true;
    }
    private bool UpgradeSuperChargeDamage(Player player, Market market)
    {
        if (player.Balance < market.SuperChargeCost || market.SuperChargeLevel >= 20) return false;

        player.Balance -= market.SuperChargeCost;
        player.SuperCharge = market.SuperChargeNextValue;
        market.SuperChargeNextValue = (int)Math.Ceiling(500 * Math.Pow(1.4, market.SuperChargeLevel));
        market.SuperChargeCost = (long)Math.Ceiling(5000 * Math.Pow(1.95, market.SuperChargeLevel));
        market.SuperChargeLevel++;
        
        return true;
    }
    private bool UpgradeEnergyCharge(Player player, Market market)
    {
        if (player.GoldBalance < market.EnergyChargeCost || market.EnergyChargeLevel >= 100) return false;

        player.GoldBalance -= market.EnergyChargeCost;
        player.EnergyCharge = market.EnergyChargeNextValue;
        market.EnergyChargeNextValue = (decimal)Math.Ceiling(2000 * Math.Pow(1.03, market.EnergyChargeLevel));
        market.EnergyChargeCost = (long)Math.Ceiling(400 * Math.Pow(1.2, market.EnergyChargeLevel));
        market.EnergyChargeLevel++;
        
        return true;
    }
    private bool UpgradeEnergyCapacity(Player player, Market market)
    {
        if (player.Balance < market.EnergyCost || market.EnergyLevel >= 100) return false;

        player.Balance -= market.EnergyCost;
        player.Energy = market.EnergyNextValue;
        market.EnergyCost = (long)Math.Ceiling(5000 * Math.Pow(1.25, market.EnergyLevel));
        market.EnergyNextValue = (int)Math.Ceiling(2000 * Math.Pow(1.03, market.EnergyLevel));
        market.EnergyLevel++;
        
        return true;
    }
    private bool UpgradeDamagePerClick(Player player, Market market)
    {
        if (player.Balance < market.DamagePerClickCost || market.DamagePerClickLevel >= 500) return false;

        player.Balance -= market.DamagePerClickCost;
        player.DamagePerClick = market.DamagePerClickNextValue;
        market.DamagePerClickCost = 
            (long)Math.Ceiling(player.DamagePerClick * 500 * Math.Pow(1.01, market.DamagePerClickLevel));
        market.DamagePerClickNextValue = (int)(market.DamagePerClickNextValue * 1.05);
        market.DamagePerClickLevel++;
        
        return true;
    }
    #endregion
}