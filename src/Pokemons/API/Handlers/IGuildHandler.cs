using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;

namespace Pokemons.API.Handlers;

public interface IGuildHandler
{
    Task<CallResult<GuildResponseDto>> CreateGuild(string guildName, long playerId);
    Task<CallResult<GuildResponseDto>> GetGuildByPlayerId(long playerId);
    Task<CallResult<bool>> SendJoinRequest(long playerId, long guildId);
    Task<CallResult<bool>> ChangeJoinRequestStatus(long playerId, long memberId, bool isConfirm);
    Task<CallResult<PopularGuilds>> GetMostPopularGuilds();
}