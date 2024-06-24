using Pokemons.Core.Enums;
using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.RatingRepository;

public interface IRatingRepository
{
    Task CreateUserRating(long playerId, LeagueType leagueType);
    Task<IEnumerable<RatingPlayerDescription>> GetLeagueRating(int leagueType, int offset);
    Task<Rating?> GetByPlayerId(long playerId);
    Task Update(Rating rating);
    Task Save(long playerId);
}