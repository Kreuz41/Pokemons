using Pokemons.Core.Enums;

namespace Pokemons.DataLayer.Cache.Models;

public class LeaguePlayerDescription : ShortPlayerDescription
{
    public int DefeatedEntities { get; set; }
    public LeagueType LeagueType { get; set; }
    public int Position { get; set; }
}