namespace Pokemons.API.Dto.Responses;

public class GuildShortDescription
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public int Rating { get; set; }
    public int MembersCount { get; set; }
}

public record PopularGuilds(IEnumerable<GuildShortDescription> Guilds);