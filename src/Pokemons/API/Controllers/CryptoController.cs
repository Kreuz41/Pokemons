using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[ApiController]
[Route("crypto/")]
public class CryptoController : ControllerBase
{
    public CryptoController(ICryptoHandler cryptoHandler)
    {
        _cryptoHandler = cryptoHandler;
    }

    private readonly ICryptoHandler _cryptoHandler;
    
    [HttpPost("buyPremium")]
    public async Task<IResult> BuyPremium()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;
        var result = await _cryptoHandler.BuyPrem(playerId);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}