using Pokemons.Core.Enums;
using Pokemons.Core.Helpers;
using Pokemons.DataLayer.Cache.Models;
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

    public async Task StartUpdateLeagueRating()
    { 
        var ratings = (await _ratingRepository.GetRatings()).ToList();
        foreach (var rating in 
                 ratings.Where(rating => 
                     rating.Player.DefeatedEntities > LeagueHelper.NeededToNextLeague(rating.Player.Rating.LeagueType)))
            rating.Player.Rating.LeagueType++;
        
        var groupedRatings = ratings.GroupBy(r => r.LeagueType)
                .Select(r => new
                {
                    LeagueType = r.Key,
                    Ratings = r.OrderByDescending(g => g.Player.DefeatedEntities).ToList()
                })
                .ToList();

        foreach (var group in groupedRatings)
        {
            for (var i = 0; i < group.Ratings.Count; i++)
            {
                group.Ratings[i].LeaguePosition = i + 1;
            }
        }

        await _ratingRepository.UpdateRange(groupedRatings.SelectMany(g => g.Ratings));
    }
}