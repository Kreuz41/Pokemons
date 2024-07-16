using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Cache.Models;

public class GuildMembers
{
    public IEnumerable<Player> Members { get; set; } = [];
}