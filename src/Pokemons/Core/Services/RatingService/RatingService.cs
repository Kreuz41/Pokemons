using Pokemons.Core.Enums;
using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.RatingRepository;

namespace Pokemons.Core.Services.RatingService;

public class RatingService : IRatingService
{
    public RatingService(IRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    private readonly IRatingRepository _ratingRepository;

    public async Task CreateRating(long playerId) =>
        await _ratingRepository.CreateUserRating(playerId, LeagueType.Beginners);

    public async Task<IEnumerable<RatingPlayerDescription>> GetLeagueRating(int leagueType, int offset) => 
        await _ratingRepository.GetLeagueRating(leagueType, offset);
}