using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("api/guild/")]
public class GuildController : ControllerBase
{
    public GuildController(IGuildHandler handler)
    {
        _handler = handler;
    }

    private readonly IGuildHandler _handler;

    [HttpPost("create")]
    public async Task<IResult> Create([FromQuery] string guildName)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var response = await _handler.CreateGuild(guildName, playerId);
        return response.Status ? Results.Ok(response) : Results.BadRequest(response);
    }

    [HttpGet("get")]
    public async Task<IResult> Get()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var response = await _handler.GetGuildByPlayerId(playerId);
        return response.Status ? Results.Ok(response) : Results.BadRequest(response);
    }

    [HttpPost("sendJoinRequest")]
    public async Task<IResult> SendRequest([FromQuery] long guildId)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var response = await _handler.SendJoinRequest(playerId, guildId);
        return response.Status ? Results.Ok(response) : Results.BadRequest(response);
    }

    [HttpPost("changeJoinRequestStatus")]
    public async Task<IResult> ChangeStatus([FromQuery] long memberId, [FromQuery] bool isConfirm)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var response = await _handler.ChangeJoinRequestStatus(playerId, memberId, isConfirm);
        return response.Status ? Results.Ok(response) : Results.BadRequest(response);
    }

    [HttpGet("getMostPopular")]
    public async Task<IResult> GetMostPopular()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var response = await _handler.GetMostPopularGuilds();
        return response.Status ? Results.Ok(response) : Results.BadRequest(response);
    }
}