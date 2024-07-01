using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;
using Pokemons.Core.Enums;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("api/market/")]
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

    [HttpPut("upgradeStat")]
    public async Task<IResult> UpgradeDamagePerClick([FromQuery] StatType type)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpgradeUserStat(playerId, type);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}