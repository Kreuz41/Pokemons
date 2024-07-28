using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Handlers;
using Pokemons.API.Jwt;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("api/auth/")]
public class AuthController : ControllerBase
{
    public AuthController(IAuthHandler handler, JwtHandler jwtHandler)
    {
        _handler = handler;
        _jwtHandler = jwtHandler;
    }

    private readonly IAuthHandler _handler;
    private readonly JwtHandler _jwtHandler;
    
    [HttpPost("startSession")]
    public async Task<IResult> StartSession([FromBody] StartSessionDto data)
    {
         if (HttpContext.Request.Headers.TryGetValue("UserId", out var userIdHeader))
        {
            if (long.TryParse(userIdHeader, out var userId))
            {
                var result = await _handler.StartSession(data, userId);

                if (!result.Status) return Results.BadRequest(result);

                var token = _jwtHandler.GetToken(userId);

                var response = CallResult.CallResult<string>.Success(token);
                return Results.Ok(response);
            }
            else
            {
                return BadRequest("Invalid UserId header value.");
            }
        }
    }

    [Authorize]
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

    [Authorize]
    [HttpGet("profile")]
    public async Task<IResult> GetProfile()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.GetProfile(playerId);
        
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [Authorize]
    [HttpPost("editProfile")]
    public async Task<IResult> EditProfile([FromBody] EditProfileDto dto)
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _handler.UpdateProfile(playerId, dto);

        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
    
    [Authorize]
    [HttpPost("endSession")]
    public async Task<IResult> EndSession()
    {
        var userId = (long)HttpContext.Items["UserId"]!;
        await _handler.EndSession(userId);

        return Results.Ok();
    }
}