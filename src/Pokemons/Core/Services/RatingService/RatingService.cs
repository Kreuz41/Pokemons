using Pokemons.Core.Enums;
using Pokemons.Core.Helpers;
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

    public async Task UpdateRating(long playerId, int defeated)
    {
        var rating = await _ratingRepository.GetByPlayerId(playerId);
        if (rating is null) return;

        if (defeated > LeagueHelper.NeededToNextLeague(rating.LeagueType))
        {
            rating.LeagueType++;
            await _ratingRepository.Update(rating);
        }
    }

    public async Task Save(long playerId) =>
        await _ratingRepository.Save(playerId);
}