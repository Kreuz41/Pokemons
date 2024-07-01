using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("api/missions/")]
public class MissionController : ControllerBase
{
    public MissionController(IMissionHandler missionHandler)
    {
        _missionHandler = missionHandler;
    }

    private readonly IMissionHandler _missionHandler;
    
    [HttpGet("get")]
    public async Task<IResult> GetMissions()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _missionHandler.GetMissions(playerId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPut("complete")]
    public async Task<IResult> CompleteMission([FromQuery] int missionId)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _missionHandler.CompleteMission(playerId, missionId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}