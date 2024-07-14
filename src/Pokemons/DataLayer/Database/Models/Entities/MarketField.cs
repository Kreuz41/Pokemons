namespace Pokemons.DataLayer.Database.Models.Entities;

public class MarketField
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal BaseValue { get; set; }
}