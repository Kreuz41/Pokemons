using System.Text.Json.Serialization;
using Pokemons.Core.Enums;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class MemberGuildStatus
{
    public long Id { get; set; }
    public MemberStatus MemberStatus { get; set; } = MemberStatus.Nothing;
    public long? GuildId { get; set; }
    [JsonIgnore] public Guild Guild { get; set; } = null!;
    public long PlayerId { get; set; }
    [JsonIgnore] public Player Player { get; set; } = null!;
}