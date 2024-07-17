using Pokemons.Core.Enums;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.GuildRepository;

namespace Pokemons.Core.Services.GuildService;

public class GuildService : IGuildService
{
    public GuildService(IGuildRepository guildRepository)
    {
        _guildRepository = guildRepository;
    }

    private readonly IGuildRepository _guildRepository;

    public event Func<long, long, Task>? RequestSend;
    public event Func<long, bool, Task>? ChangeRequestStatus;

    public async Task CreateGuild(string name, long founderId)
    {
        var createdGuild = await _guildRepository.CreateGuild(name, founderId);
        await _guildRepository.ChangeGuildStatus(founderId, MemberStatus.Founder);
        await _guildRepository.ChangeGuildId(founderId, createdGuild.Id);
    }

    public async Task CreateMemberStatus(long playerId) =>
        await _guildRepository.CreateMemberGuildStatus(playerId);

    public async Task<bool> IsPlayerInGuild(long playerId)
    {
        var guildMember = await _guildRepository.GetGuildMember(playerId);
        return guildMember?.MemberStatus != MemberStatus.Nothing;
    }

    public async Task<Guild?> GetGuildByPlayerId(long playerId) =>
        await _guildRepository.GetGuildByPlayerId(playerId);
    
    public async Task<IEnumerable<Player>> GetGuildMembers(long guildId) => 
        await _guildRepository.GetAllGuildMembers(guildId);

    public async Task SendRequestToJoin(long playerId, long guildId)
    {
        var member = await _guildRepository.GetGuildMember(playerId);
        if (member is null) return;
        
        member.MemberStatus = MemberStatus.Waiting;
        member.GuildId = guildId;

        await _guildRepository.UpdateMember(member);
        
        OnRequestSend(playerId, guildId);
    }

    public async Task Save(long playerId) =>
        await _guildRepository.Save(playerId);

    public async Task<bool> IsPlayerAdmin(long playerId)
    {
        var member = await _guildRepository.GetGuildMember(playerId);
        return member?.MemberStatus is MemberStatus.Admin or MemberStatus.Founder;
    }

    public async Task ChangeJoinRequestStatus(long memberId, bool isConfirm)
    {
        var member = await _guildRepository.GetGuildMember(memberId);
        if (member is null) return;
        
        member.MemberStatus = isConfirm ? MemberStatus.Member : MemberStatus.Nothing;

        if (isConfirm)
        {
            var guild = await _guildRepository.GetGuildByPlayerId(memberId);
            if (guild is not null)
            {
                guild.PlayersCount++;
                await _guildRepository.SaveGuild(guild);
            }
        }
        else
            member.GuildId = null;
        
        await _guildRepository.UpdateMember(member);
        
        OnChangeRequestStatus(memberId, isConfirm);
    }

    public async Task<IEnumerable<Guild>> GetMostPopularGuilds() =>
        await _guildRepository.GetPopularGuilds();

    protected virtual void OnRequestSend(long playerId, long guildId)
    {
        RequestSend?.Invoke(playerId, guildId);
    }

    protected virtual void OnChangeRequestStatus(long playerId, bool isConfirmed)
    {
        ChangeRequestStatus?.Invoke(playerId, isConfirmed);
    }
}