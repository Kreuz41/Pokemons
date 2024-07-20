namespace Pokemons.DataLayer.Database.Models.Entities;

public class News
{
    public long Id { get; set; }
    public bool IsRead { get; set; }
    
    public long ActiveNewsId { get; set; }
    public ActiveNews ActiveNews { get; set; } = null!;
    public long PlayerId { get; set; }
    public Player Player { get; set; } = null!;
}