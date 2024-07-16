using System.Text.Json.Serialization;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Guild
{
    public long Id { get; set; }
    public int PlayersCount { get; set; } = 1;
    public string Name { get; set; } = null!;
    public int Balance { get; set; }
    public int TotalBalance { get; set; }
    public long GuildMasterId { get; set; }
    [JsonIgnore] public ICollection<MemberGuildStatus> Members { get; set; } = null!;
}