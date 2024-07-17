using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.GuildRepos;

public class GuildDatabaseRepository : IGuildDatabaseRepository
{
    public GuildDatabaseRepository(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<Guild> CreateGuild(Guild guild)
    {
        await _unitOfWork.BeginTransaction();
        var entity = await _context.Guilds.AddAsync(guild);
        await _unitOfWork.CommitTransaction();
        return entity.Entity;
    }

    public async Task CreateMemberStatus(MemberGuildStatus memberGuildStatus)
    {
        await _unitOfWork.BeginTransaction();
        await _context.MemberGuildStatus.AddAsync(memberGuildStatus);
        await _unitOfWork.CommitTransaction();
    }

    public async Task<MemberGuildStatus?> GetGuildMember(long playerId) =>
        await _context.MemberGuildStatus.FirstOrDefaultAsync(m => m.PlayerId == playerId);

    public async Task<Guild?> GetGuildByPlayerId(long playerId) =>
        (await _context.MemberGuildStatus
            .Include(g => g.Guild)
            .FirstOrDefaultAsync(m => m.PlayerId == playerId))?
        .Guild;

    public async Task<IEnumerable<Player>> GetAllMembers(long guildId) =>
        await _context.MemberGuildStatus
            .Include(m => m.Player)
            .Include(m => m.Player.GuildStatus)
            .Where(m => m.GuildId == guildId)
            .Select(m => m.Player)
            .ToListAsync();

    public async Task Save(MemberGuildStatus memberStatus)
    {
        await _unitOfWork.BeginTransaction();
        var trackedEntity = _context.ChangeTracker.Entries<MemberGuildStatus>()
            .FirstOrDefault(e => e.Entity.Id == memberStatus.Id);
        if (trackedEntity != null)
            _context.Entry(trackedEntity.Entity).State = EntityState.Detached;
        _context.Entry(memberStatus).State = EntityState.Modified;
        _context.Attach(memberStatus);
        
        _context.MemberGuildStatus.Update(memberStatus);
        await _unitOfWork.CommitTransaction();
    }

    public async Task SaveGuild(Guild guild)
    {
        await _unitOfWork.BeginTransaction();
        _context.Guilds.Update(guild);
        await _unitOfWork.CommitTransaction();
    }

    public async Task<IEnumerable<Guild>> GetPopularsGuild() =>
        await _context.Guilds
            .OrderBy(g => g.PlayersCount)
            .Take(100)
            .ToListAsync();

    public async Task<Guild?> GetById(long guildId) =>
        await _context.Guilds.FirstOrDefaultAsync(g => g.Id == guildId);
}