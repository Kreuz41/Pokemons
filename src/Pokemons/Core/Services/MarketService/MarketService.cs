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
        market.SuperChargeCooldownCost = (long)(market.SuperChargeCooldownCost * 2.6);
        if (market.SuperChargeCooldownLevel == 0)
            player.LastSuperChargeActivatedTime = _timeProvider.Now();
        
        market.SuperChargeCooldownLevel++;
        market.SuperChargeCooldownNextValue += 0.25m;
        
        return true;
    }
    private bool UpgradeSuperChargeDamage(Player player, Market market)
    {
        if (player.Balance < market.SuperChargeCost || market.SuperChargeLevel >= 20) return false;

        player.Balance -= market.SuperChargeCost;
        player.SuperCharge = market.SuperChargeNextValue;
        market.SuperChargeNextValue = (int)(market.SuperChargeNextValue * 1.03);
        market.SuperChargeCost = (long)(market.SuperChargeCost * 2.6m);
        market.SuperChargeLevel++;
        
        return true;
    }
    private bool UpgradeEnergyCharge(Player player, Market market)
    {
        if (player.Balance < market.EnergyChargeCost || market.EnergyChargeLevel >= 100) return false;

        player.Balance -= market.EnergyChargeCost;
        player.EnergyCharge = market.EnergyChargeNextValue;
        market.EnergyChargeNextValue = 3600m / player.Energy;
        market.EnergyChargeCost = (long)(market.EnergyChargeCost * 1.3);
        market.EnergyChargeLevel++;
        
        return true;
    }
    private bool UpgradeEnergyCapacity(Player player, Market market)
    {
        if (player.Balance < market.EnergyCost || market.EnergyLevel >= 100) return false;

        player.Balance -= market.EnergyCost;
        player.Energy = market.EnergyNextValue;
        market.EnergyCost = (long)(market.EnergyCost * 1.3);
        market.EnergyNextValue = (int)(market.EnergyNextValue * 1.04);
        market.EnergyLevel++;
        
        return true;
    }
    private bool UpgradeDamagePerClick(Player player, Market market)
    {
        if (player.Balance < market.DamagePerClickCost || market.DamagePerClickLevel >= 500) return false;

        player.Balance -= market.DamagePerClickCost;
        player.DamagePerClick = market.DamagePerClickNextValue;
        market.DamagePerClickCost = 
            (long)(market.DamagePerClickNextValue * 1000 * Math.Pow(1.015, market.DamagePerClickLevel));
        market.DamagePerClickNextValue = (int)(market.DamagePerClickNextValue * 1.02);
        market.DamagePerClickLevel++;
        
        return true;
    }
    #endregion
}