using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.PlayerRepos;

public class PlayerDatabaseRepository : IPlayerDatabaseRepository
{
    public PlayerDatabaseRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Player?> GetById(long id) =>
        await _context.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task CreatePlayer(Player player)
    {
        await _unitOfWork.BeginTransaction();
        await _context.Players.AddAsync(player);
        await _unitOfWork.CommitTransaction();
    }

    public async Task UpdatePlayer(Player player)
    {
        await _unitOfWork.BeginTransaction();
        _context.Update(player);
        await _unitOfWork.CommitTransaction();
    }

    public async Task UpdatePlayers(IEnumerable<Player> players)
    {
        await _unitOfWork.BeginTransaction();
        _context.Players.UpdateRange(players);
        await _unitOfWork.CommitTransaction();
    }
}