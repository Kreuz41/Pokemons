using Microsoft.EntityFrameworkCore;
using Pokemons.Core.Enums;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.RatingRepos;

public class RatingDatabaseRepository : IRatingDatabaseRepository
{
    public RatingDatabaseRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public async Task Create(Rating rating)
    {
        await _unitOfWork.BeginTransaction();
        await _context.AddAsync(rating);
        await _unitOfWork.CommitTransaction();
    }

    public async Task<long> GetMaxPositionInGlobalRating() =>
        await _context.Set<Rating>()
            .MaxAsync(r => (long?)r.GlobalRatingPosition) 
        ?? 0;

    public async Task<long> GetMaxPositionInLeague(LeagueType leagueType) =>
        await _context.Set<Rating>().Where(r => r.LeagueType == leagueType)
            .MaxAsync(r => (long?)r.LeaguePosition) 
        ?? 0;

    public async Task UpdateRatings(IEnumerable<Rating> ratings)
    {
        await _unitOfWork.BeginTransaction();
        _context.Rating.UpdateRange(ratings);
        await _unitOfWork.CommitTransaction();
    }

    public async Task<IEnumerable<Rating>> GetLeagueRating(int leagueType, int offset)
    {
        const int ratingLimit = 10;
        var ratings = await _context.Rating
            .Include(r => r.Player)
            .Where(r => (int)r.LeagueType == leagueType 
                        && ratingLimit * offset < r.LeaguePosition 
                        && ratingLimit * (offset + 1) > r.LeaguePosition)
            .OrderBy(r => r.LeaguePosition)
            .ToListAsync();

        return ratings;
    }

    public async Task<Rating?> GetPlayerRating(long playerId) =>
        await _context.Rating.FirstOrDefaultAsync(r => r.PlayerId == playerId);

    public async Task<IEnumerable<Rating>> GetAllRatings() =>
        await _context.Rating
            .Include(r => r.Player)
            .AsNoTracking()
            .ToListAsync();
}