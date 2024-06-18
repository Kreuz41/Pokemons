using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;
using Pokemons.Core.Enums;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("market/")]
public class MarketController : ControllerBase
{
    public MarketController(IMarketHandler handler)
    {
        _handler = handler;
    }

    private readonly IMarketHandler _handler;
    
    [HttpGet("get")]
    public async Task<IResult> Get()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.GetMarketByUserId(playerId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPut("upgradeDamagePerClick")]
    public async Task<IResult> UpgradeDamagePerClick()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpgradeUserStat(playerId, StatType.DamagePerClick);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPut("upgradeEnergyCapacity")]
    public async Task<IResult> UpgradeEnergyCapacity()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpgradeUserStat(playerId, StatType.EnergyCapacity);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPut("upgradeEnergyCharge")]
    public async Task<IResult> UpgradeEnergyCharge()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpgradeUserStat(playerId, StatType.EnergyCharge);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPut("upgradeSuperChargeDamage")]
    public async Task<IResult> UpgradeSuperChargeDamage()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpgradeUserStat(playerId, StatType.SuperChargeDamage);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPut("upgradeSuperChargeCooldown")]
    public async Task<IResult> UpgradeSuperChargeCooldown()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpgradeUserStat(playerId, StatType.SuperChargeCooldown);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}