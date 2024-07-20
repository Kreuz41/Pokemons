using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("crypto/")]
public class CryptoController : ControllerBase
{
    public CryptoController(ICryptoHandler cryptoHandler, IAuthHandler authHandler)
    {
        _cryptoHandler = cryptoHandler;
        _authHandler = authHandler;
    }

    private readonly ICryptoHandler _cryptoHandler;
    private readonly IAuthHandler _authHandler;
    
    [HttpPost("buyPremium")]
    public async Task<IResult> BuyPremium()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _cryptoHandler.BuyPrem(playerId);
        if (!result.Status) return Results.BadRequest(result);
        var response = await _authHandler.GetProfile(playerId);
        return response.Status ? Results.Ok(response) : Results.BadRequest(response);
    }
}