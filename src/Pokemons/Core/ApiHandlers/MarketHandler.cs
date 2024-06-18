using System.Reflection;
using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Enums;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.MarketRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;

namespace Pokemons.Core.ApiHandlers;

public class MarketHandler : IMarketHandler
{
    public MarketHandler(IMapper mapper, IMarketRepository marketRepository, IPlayerRepository playerRepository)
    {
        _marketRepository = marketRepository;
        _playerRepository = playerRepository;
        _mapper = mapper;
    }

    private readonly IMarketRepository _marketRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMapper _mapper;
    
    public async Task<CallResult<MarketResponseDto>> GetMarketByUserId(long playerId)
    {
        var market = await _marketRepository.GetMarketByPlayerId(playerId);
        return CallResult<MarketResponseDto>.Success(_mapper.Map<MarketResponseDto>(market));
    }

    public async Task<CallResult<MarketResponseDto>> UpgradeUserStat(long playerId, StatType type)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return CallResult<MarketResponseDto>.Failure("Player does not exist");

        var market = await _marketRepository.GetMarketByPlayerId(playerId);

        if (!TryUpgradeStat(player, market, type))
            return CallResult<MarketResponseDto>.Failure("Not enough money or invalid stat type");

        await _playerRepository.Update(player);
        await _marketRepository.Update(market);

        return CallResult<MarketResponseDto>.Success(_mapper.Map<MarketResponseDto>(market));
    }
    
    private bool TryUpgradeStat(Player player, Market market, StatType type)
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
    private bool UpgradeSuperChargeCooldown(Player player, Market market)
    {
        if (player.Balance < market.SuperChargeCooldownCost) return false;

        player.Balance -= market.SuperChargeCooldownCost;
        player.SuperChargeCooldown += 0.02m;
        market.SuperChargeCooldownCost *= 2;
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
}