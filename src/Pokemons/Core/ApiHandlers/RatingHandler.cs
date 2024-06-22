using Pokemons.API.CallResult;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.RatingService;
using Pokemons.DataLayer.Cache.Models;

namespace Pokemons.Core.ApiHandlers;

public class RatingHandler : IRatingHandler
{
    public RatingHandler(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    private readonly IRatingService _ratingService;

    public async Task<CallResult<IEnumerable<RatingPlayerDescription>>> GetLeagueRating(int leagueType, int offset) =>
        CallResult<IEnumerable<RatingPlayerDescription>>.Success(
            await _ratingService.GetLeagueRating(leagueType, offset));
}