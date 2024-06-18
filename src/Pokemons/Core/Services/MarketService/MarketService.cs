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

    #region UpgradeStats
    private bool UpgradeSuperChargeCooldown(Player player, Market market)
    {
        if (player.Balance < market.SuperChargeCooldownCost) return false;

        player.Balance -= market.SuperChargeCooldownCost;
        player.SuperChargeCooldown += 0.02m;
        market.SuperChargeCooldownCost *= 2;
        if (market.SuperChargeCooldownLevel == 0)
            player.LastSuperChargeActivatedTime = _timeProvider.Now();
        
        market.SuperChargeCooldownLevel++;
        
        return true;
    }
    private bool UpgradeSuperChargeDamage(Player player, Market market)
    {
        if (player.Balance < market.SuperChargeCost) return false;

        player.Balance -= market.SuperChargeCost;
        player.SuperCharge += 1000;
        market.SuperChargeCost *= 2;
        market.SuperChargeLevel++;
        
        return true;
    }
    private bool UpgradeEnergyCharge(Player player, Market market)
    {
        if (player.Balance < market.EnergyChargeCost) return false;

        player.Balance -= market.EnergyChargeCost;
        player.EnergyCharge += 0.02m;
        market.EnergyChargeCost *= 2;
        market.EnergyChargeLevel++;
        
        return true;
    }
    private bool UpgradeEnergyCapacity(Player player, Market market)
    {
        if (player.Balance < market.EnergyCost) return false;

        player.Balance -= market.EnergyCost;
        player.Energy += 500;
        market.EnergyCost *= 2;
        market.EnergyLevel++;
        
        return true;
    }
    private bool UpgradeDamagePerClick(Player player, Market market)
    {
        if (player.Balance < market.DamagePerClickCost) return false;

        player.Balance -= market.DamagePerClickCost;
        player.DamagePerClick++;
        market.DamagePerClickCost *= 2;
        market.DamagePerClickLevel++;
        
        return true;
    }
    #endregion
}