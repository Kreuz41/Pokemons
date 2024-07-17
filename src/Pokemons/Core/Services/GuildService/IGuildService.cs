using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.GuildService;

public interface IGuildService
{
    Task CreateGuild(string name, long founderId);
    Task CreateMemberStatus(long playerId);
    Task<bool> IsPlayerInGuild(long playerId);
    Task<Guild?> GetGuildByPlayerId(long playerId);
    Task<IEnumerable<Player>> GetGuildMembers(long guildId);
    Task SendRequestToJoin(long playerId, long guildId);
    Task Save(long playerId);
    Task<bool> IsPlayerAdmin(long playerId);
    Task ChangeJoinRequestStatus(long memberId, bool isConfirm);
    Task<IEnumerable<Guild>> GetMostPopularGuilds();
}