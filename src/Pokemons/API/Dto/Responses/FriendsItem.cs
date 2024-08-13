namespace Pokemons.API.Dto.Responses;

public class FriendsItem
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Surname { get; set; }
    public string? PhotoUrl { get; set; }
    public int Inline { get; set; }
    public int LeagueType { get; set; }
    public int Level { get; set; }
    public int DefeatedEntities { get; set; }
    public int ProfitNumber { get; set; }
    public int RefCount { get; set; }
}