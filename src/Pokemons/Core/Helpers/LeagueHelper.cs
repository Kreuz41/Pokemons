using Pokemons.Core.Enums;

namespace Pokemons.Core.Helpers;

public static class LeagueHelper
{
    public static int NeededToNextLeague(LeagueType type) =>
        type switch
        {
            LeagueType.Beginners => 10,
            LeagueType.Researchers => 30,
            LeagueType.Coach => 60,
            LeagueType.Masters => 100,
            LeagueType.Experts => 150,
            LeagueType.Legends => 200,
            LeagueType.Grandmasters => 300,
            LeagueType.Champions => 500,
            LeagueType.SupremeMasters => 750,
            _ => int.MaxValue
        };
}