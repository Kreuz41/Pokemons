namespace Pokemons.DataLayer.Cache.Models;

public class RatingPlayerDescription : ShortPlayerDescription
{
    public long Position { get; set; }
    public int DefeatedEntities { get; set; }
    public int TotalDamage { get; set; }
    public int Level { get; set; }
}