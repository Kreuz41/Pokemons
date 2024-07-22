using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.GuildService;
using Pokemons.Core.Services.PlayerService;

namespace Pokemons.Core.ApiHandlers;

public class GuildHandler : IGuildHandler
{
    public GuildHandler(IGuildService guildService, IPlayerService playerService)
    {
        _guildService = guildService;
        _playerService = playerService;
    }

    private readonly IGuildService _guildService;
    private readonly IPlayerService _playerService;
    
    public async Task<CallResult<GuildResponseDto>> CreateGuild(string guildName, long playerId)
    {
        if (await _guildService.IsPlayerInGuild(playerId))
            return CallResult<GuildResponseDto>.Failure("You cannot create guild because you are member another one");
        var player = await _playerService.GetPlayer(playerId);
        if (player?.Level < 5)
            return CallResult<GuildResponseDto>.Failure("You cannot create guild before lvl 5");

        await _guildService.CreateGuild(guildName, playerId);
        
        return await GetGuildByPlayerId(playerId);
    }

    public async Task<CallResult<GuildResponseDto>> GetGuildByPlayerId(long playerId)
    {
        var guild = await _guildService.GetGuildByPlayerId(playerId);

        if (guild is null)
            return CallResult<GuildResponseDto>.Failure("Guild doesn't exist");

        var members = await _guildService.GetGuildMembers(guild.Id);

        return CallResult<GuildResponseDto>.Success(new GuildResponseDto
        {
            Id = guild.Id,
            Name = guild.Name,
            MembersCount = guild.PlayersCount,
            Balance = guild.Balance,
            TotalBalance = guild.TotalBalance,
            Status = members.FirstOrDefault(m => m.Id == playerId).GuildStatus.MemberStatus,
            Members = members.Select(p => 
                new GuildMember(p.Id, 
                    p.Name, 
                    p.Surname, 
                    p.PhotoUrl, 
                    p.DefeatedEntities, 
                    p.TotalDamage, 
                    p.Level,
                    p.GuildStatus.MemberStatus))
        });
    }

    public async Task<CallResult<bool>> SendJoinRequest(long playerId, long guildId)
    {
        if (await _guildService.IsPlayerInGuild(playerId))
            return CallResult<bool>.Failure("You cannot send join request because you are member another one");

        if (!await _guildService.IsGuildExist(guildId))
            return CallResult<bool>.Failure("Guild doesn't exist");

        await _guildService.SendRequestToJoin(playerId, guildId);

        return CallResult<bool>.Success(true);
    }

    public async Task<CallResult<bool>> ChangeJoinRequestStatus(long playerId, long memberId, bool isConfirm)
    {
        if (!await _guildService.IsPlayerAdmin(playerId))
            return CallResult<bool>.Failure("You have not permissions for this");

        await _guildService.ChangeJoinRequestStatus(memberId, isConfirm);

        return CallResult<bool>.Success(true);
    }

    public async Task<CallResult<PopularGuilds>> GetMostPopularGuilds()
    {
        var guilds = await _guildService.GetMostPopularGuilds();
        var response = new PopularGuilds(guilds.Select(g => new GuildShortDescription
        {
            Id = g.Id,
            MembersCount = g.PlayersCount,
            Name = g.Name,
            Rating = 0
        }));
        return CallResult<PopularGuilds>.Success(response);
    }
}