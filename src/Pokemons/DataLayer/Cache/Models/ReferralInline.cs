using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Cache.Models;

public class ReferralInline
{
    public Player Player { get; set; } = null!;
    public int Inline { get; set; }
}