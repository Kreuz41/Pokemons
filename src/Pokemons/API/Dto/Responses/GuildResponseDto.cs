using Pokemons.Core.Enums;

namespace Pokemons.API.Dto.Responses;

public class GuildResponseDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public int Balance { get; set; }
    public int TotalBalance { get; set; }
    public int MembersCount { get; set; }
    public IEnumerable<GuildMember> Members { get; set; } = null!;
    public MemberStatus Status { get; set; }
}

public record GuildMember(
    long Id,
    string? Name,
    string? Surname,
    string? PhotoUrl,
    int DefeatedEntities,
    int TotalDamage,
    int Level,
    MemberStatus Status);