using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Handlers;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController : ControllerBase
{
    public AuthController(IAuthHandler handler)
    {
        _handler = handler;
    }

    private readonly IAuthHandler _handler;
    
    [HttpPost("startSession")]
    public async Task<IResult> StartSession([FromBody] EditProfileDto data)
    {
        var userId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.StartSession(userId, data);

        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpGet("tapperConfig")]
    public async Task<IResult> GetTapperConfig()
    {
        var userId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.GetTapperConfig(userId);
        
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    
    [HttpPost("createUser")]
    public async Task<IResult> CreateProfile([FromBody] CreateUserModel dto)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.CreateUser(dto, playerId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpGet("profile")]
    public async Task<IResult> GetProfile()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.GetProfile(playerId);
        
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPost("editProfile")]
    public async Task<IResult> EditProfile([FromBody] EditProfileDto dto)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpdateProfile(playerId, dto);

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