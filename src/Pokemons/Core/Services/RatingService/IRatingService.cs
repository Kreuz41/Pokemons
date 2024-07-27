using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.RatingService;

public interface IRatingService
{
    Task CreateRating(long playerId);
    Task<IEnumerable<RatingPlayerDescription>> GetLeagueRating(int leagueType, int offset);
    Task UpdateRating(long playerId, int defeated);
    Task Save(long playerId);
    Task StartUpdateLeagueRating();
    Task<Rating?> GetPlayerRating(long playerId);
}