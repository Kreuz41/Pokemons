using Pokemons.Core.Enums;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.GuildRepository;

public interface IGuildRepository
{
    Task<Guild> CreateGuild(string guildName, long founderId);
    Task CreateMemberGuildStatus(long playerId);
    Task<MemberGuildStatus?> GetGuildMember(long playerId);
    Task<Guild?> GetGuildByPlayerId(long playerId);
    Task<IEnumerable<Player>> GetAllGuildMembers(long guildId);
    Task ChangeGuildStatus(long playerId, long guildId, MemberStatus status);
    Task Save(long playerId);
    Task UpdateMember(MemberGuildStatus member);
    Task SaveGuild(Guild guild);
    Task<IEnumerable<Guild>> GetPopularGuilds();
}