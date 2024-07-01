namespace Pokemons.DataLayer.Database.Models.Entities;

public class Mission
{
    public long Id { get; set; }
    public DateTime? CompleteTime { get; set; }
    
    public long PlayerId { get; set; }
    public Player Player { get; set; } = null!;
    
    public int ActiveMissionId { get; set; }
    public ActiveMission ActiveMission { get; set; } = null!;
}