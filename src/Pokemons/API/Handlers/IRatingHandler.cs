using Pokemons.DataLayer.Cache.Models;

namespace Pokemons.API.Handlers;

public interface IRatingHandler
{
    Task<CallResult.CallResult<IEnumerable<RatingPlayerDescription>>> GetLeagueRating(int leagueType, int offset);
}