using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("auth/")]
public class AuthController : ControllerBase
{
    public AuthController(IAuthHandler handler)
    {
        _handler = handler;
    }

    private readonly IAuthHandler _handler;

    [HttpPost("createUser")]
    public async Task<IResult> CreateUser([FromBody] StartSessionDto data)
    {
        var userId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.CreateUser(data, userId);
        return result.Status ? Results.Created() : Results.Conflict(result);
    }
    
    [HttpPost("startSession")]
    public async Task<IResult> StartSession([FromBody] StartSessionDto data)
    {
        var userId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.StartSession(userId, data);

        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [HttpPost("endSession")]
    public async Task<IResult> EndSession()
    {
        var userId = (long)HttpContext.Items["UserId"]!;
        await _handler.EndSession(userId);

        return Results.Ok();
    }
}