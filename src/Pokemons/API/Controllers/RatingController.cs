using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pokemons.API.Handlers;

namespace Pokemons.API.Controllers;

[Authorize]
[ApiController]
[Route("api/rating/")]
public class RatingController : ControllerBase
{
    public RatingController(IRatingHandler handler)
    {
        _handler = handler;
    }

    private readonly IRatingHandler _handler;

    [HttpGet("leagueRatingInfo")]
    public async Task<IResult> GetLeagueRatingInfo([FromQuery] int leagueType, [FromQuery] int offset)
    {
        var id = (long)HttpContext.Items["UserId"]!;

        var result = await _handler.GetLeagueRating(leagueType, offset);
        
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }
}