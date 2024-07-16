using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.GuildRepos;

public interface IGuildDatabaseRepository
{
    Task<Guild> CreateGuild(Guild guild);
    Task CreateMemberStatus(MemberGuildStatus memberGuildStatus);
    Task<MemberGuildStatus?> GetGuildMember(long playerId);
    Task<Guild?> GetGuildByPlayerId(long playerId);
    Task<IEnumerable<Player>> GetAllMembers(long guildId);
    Task Save(MemberGuildStatus memberStatus);
    Task SaveGuild(Guild guild);
}