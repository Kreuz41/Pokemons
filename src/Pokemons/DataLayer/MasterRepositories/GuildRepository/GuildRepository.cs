using Pokemons.Core.Enums;
using Pokemons.DataLayer.Cache.Models;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.GuildRepos;

namespace Pokemons.DataLayer.MasterRepositories.GuildRepository;

public class GuildRepository : IGuildRepository
{
    public GuildRepository(IGuildDatabaseRepository guildDatabaseRepository, ICacheRepository cacheRepository)
    {
        _guildDatabaseRepository = guildDatabaseRepository;
        _cacheRepository = cacheRepository;
    }

    private readonly IGuildDatabaseRepository _guildDatabaseRepository;
    private readonly ICacheRepository _cacheRepository;

    public async Task<Guild> CreateGuild(string guildName, long founderId) =>
        await _guildDatabaseRepository.CreateGuild(new Guild
        {
            Name = guildName,
            GuildMasterId = founderId
        });

    public async Task CreateMemberGuildStatus(long playerId) =>
        await _guildDatabaseRepository.CreateMemberStatus(new MemberGuildStatus
        {
            PlayerId = playerId
        });

    public async Task<MemberGuildStatus?> GetGuildMember(long playerId)
    {
        var member = await _cacheRepository.GetMember<MemberGuildStatus>(playerId.ToString());
        if (member is not null)
            return member;

        member = await _guildDatabaseRepository.GetGuildMember(playerId);
        if (member is null) return null;

        await _cacheRepository.SetMember(playerId.ToString(), member);
        return member;
    }

    public async Task<Guild?> GetGuildByPlayerId(long playerId)
    {
        var member = await GetGuildMember(playerId);
        if (member is null)
            return null;
        
        var guild = await _cacheRepository.GetMember<Guild>(member.GuildId.ToString()!);
        if (guild is not null)
            return guild;

        guild = await _guildDatabaseRepository.GetGuildByPlayerId(playerId);
        if (guild is null) return null;

        await _cacheRepository.SetMember(guild.Id.ToString(), guild, 5);
        return guild;
    }

    public async Task<IEnumerable<Player>> GetAllGuildMembers(long guildId)
    {
        var cache = await _cacheRepository.GetMember<GuildMembers>(guildId.ToString());
        if (cache is not null) return cache.Members;

        var members = await _guildDatabaseRepository.GetAllMembers(guildId);
        await _cacheRepository.SetMember(guildId.ToString(), new GuildMembers
        {
            Members = members
        });

        return members;
    }

    public async Task ChangeGuildStatus(long playerId, long guildId, MemberStatus founder)
    {
        var memberStatus = await GetGuildMember(playerId);
        if (memberStatus is null) return;
        memberStatus.MemberStatus = founder;
        memberStatus.GuildId = guildId;
        await UpdateMember(memberStatus);
    }

    public async Task UpdateMember(MemberGuildStatus member)
    {
        await _cacheRepository.SetMember(member.PlayerId.ToString(), member);
        await _guildDatabaseRepository.Save(member);
    }

    public async Task SaveGuild(Guild guild)
    {
        await _cacheRepository.SetMember(guild.Id.ToString(), guild, 5);
        await _guildDatabaseRepository.SaveGuild(guild);
    }

    public async Task<IEnumerable<Guild>> GetPopularGuilds() =>
        await _guildDatabaseRepository.GetPopularsGuild();

    public async Task Save(long playerId)
    {
        var member = await _cacheRepository.DeleteMember<MemberGuildStatus>(playerId.ToString());
        if (member is not null)
            await _guildDatabaseRepository.Save(member);
    }
}