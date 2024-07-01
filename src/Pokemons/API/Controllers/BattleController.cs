using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("api/battle/")]
public class BattleController : ControllerBase
{
    public BattleController(IBattleHandler handler)
    {
        _handler = handler;
    }

    private readonly IBattleHandler _handler;
    
    [HttpPost("commitDamage")]
    public async Task<IResult> CommitDamage(CommitDamageDto dto)
    {
        var id = (long)HttpContext.Items["UserId"]!;
        
        var result = await _handler.CommitDamage(dto, id);
        
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPost("superCharge")]
    public async Task<IResult> UseSuperCharge()
    {
        var id = (long)HttpContext.Items["UserId"]!;

        var result = await _handler.UseSuperCharge(id);

        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}