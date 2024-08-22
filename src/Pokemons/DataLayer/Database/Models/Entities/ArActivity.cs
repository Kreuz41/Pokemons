
namespace Pokemons.DataLayer.Database.Models.Entities;
public class ArActivity
{
    public long Id { get; set; } // Primary Key
    public long PlayerId { get; set; } // Foreign Key
    public DateTime LastCoinCollectedAt { get; set; }
    public int Energy { get; set; } = 1000;
    public int TotalCoinsCollected { get; set; } = 0;
    public Player Player { get; set; } = null!;// Навигационное свойство
}
