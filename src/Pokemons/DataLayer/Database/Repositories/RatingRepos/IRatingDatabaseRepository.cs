using Pokemons.Core.Enums;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.RatingRepos;

public interface IRatingDatabaseRepository
{
    Task Create(Rating rating);
    Task<long> GetMaxPositionInGlobalRating();
    Task<long> GetMaxPositionInLeague(LeagueType leagueType);
    Task UpdateRatings(IEnumerable<Rating> ratings);
    Task<IEnumerable<Rating>> GetLeagueRating(int leagueType, int offset);
    Task<Rating?> GetPlayerRating(long playerId);
}