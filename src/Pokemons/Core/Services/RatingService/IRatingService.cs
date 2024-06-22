using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.RatingService;

public interface IRatingService
{
    Task CreateRating(long playerId);
    Task<IEnumerable<RatingPlayerDescription>> GetLeagueRating(int leagueType, int offset);
}