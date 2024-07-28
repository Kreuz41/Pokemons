using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[Authorize]
[ApiController]
[Route("api/referral/")]
public class ReferralController : ControllerBase
{
    public ReferralController(IReferralHandler handler)
    {
        _handler = handler;
    }

    private readonly IReferralHandler _handler;

    [HttpGet("getReferralInfo")]
    public async Task<IResult> GetReferralInfo()
    {
        var playerId = (long)HttpContext.Items["UserId"]!;

        var result = await _handler.GetReferralInfo(playerId);
        
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}