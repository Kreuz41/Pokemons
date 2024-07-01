using Pokemons.Core.Enums;
using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.RatingRepos;

namespace Pokemons.DataLayer.MasterRepositories.RatingRepository;

public class RatingRepository : IRatingRepository
{
    public RatingRepository(IRatingDatabaseRepository databaseRepository, ICacheRepository cacheRepository)
    {
        _databaseRepository = databaseRepository;
        _cacheRepository = cacheRepository;
    }

    private readonly IRatingDatabaseRepository _databaseRepository;
    private readonly ICacheRepository _cacheRepository;

    public async Task CreateUserRating(long playerId, LeagueType leagueType)
    {
        var rating = new Rating
        {
            LeagueType = leagueType,
            PlayerId = playerId,
            LeaguePosition = await _databaseRepository.GetMaxPositionInLeague(leagueType) + 1,
            GlobalRatingPosition = await _databaseRepository.GetMaxPositionInGlobalRating() + 1
        };

        await _databaseRepository.Create(rating);
    }

    public async Task<IEnumerable<RatingPlayerDescription>> GetLeagueRating(int leagueType, int offset)
    {
        var cacheResult =
            await _cacheRepository.GetMember<IEnumerable<RatingPlayerDescription>>(
                GetLeagueCacheKey(leagueType, offset));
        if (cacheResult is not null) return cacheResult;
        
        var rating = await _databaseRepository.GetLeagueRating(leagueType, offset);
        var leagueRating = rating as Rating[] ?? rating.ToArray();
        var descriptions = leagueRating.Select(r => new RatingPlayerDescription
        {
            Name = r.Player.Name,
            Surname = r.Player.Surname,
            PhotoUrl = r.Player.PhotoUrl,
            Username = r.Player.Username,
            Position = r.LeaguePosition,
        }).ToList();
        
        await _cacheRepository.SetMember(leagueType.ToString(), descriptions,
            keyPattern: () => GetLeagueCacheKey(leagueType, offset));
        
        return descriptions;
    }

    public async Task<Rating?> GetByPlayerId(long playerId)
    {
        var rating = await _cacheRepository.GetMember<Rating>(playerId.ToString());
        if (rating is not null) return rating;
        
        rating = await _databaseRepository.GetPlayerRating(playerId);
        if (rating is null) return null;
        await _cacheRepository.SetMember(playerId.ToString(), rating);
        return rating;
    }

    public async Task Update(Rating rating)
    {
        await _cacheRepository.SetMember(rating.PlayerId.ToString(), rating);
        await _databaseRepository.UpdateRatings([rating]);
    }

    public async Task Save(long playerId)
    {
        var rating = await _cacheRepository.DeleteMember<Rating>(playerId.ToString());
        await _databaseRepository.UpdateRatings([rating]);
    }

    public async Task<IEnumerable<Rating>> GetRatings() =>
        await _databaseRepository.GetAllRatings();

    public async Task UpdateRange(IEnumerable<Rating> ratings) =>
        await _databaseRepository.UpdateRatings(ratings);

    public async Task FastUpdate(Rating rating) =>
        await _cacheRepository.SetMember(rating.PlayerId.ToString(), rating);

    private static string GetLeagueCacheKey(int leagueType, int offset) =>
        $"League:{leagueType}:{offset}";
}