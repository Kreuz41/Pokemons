using System.Text.Json.Serialization;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class ActiveMission
{
    public int Id { get; set; }
    public bool IsDifficult { get; set; }
    public int Reward { get; set; }
    public bool IsEnded { get; set; }
    public DateTime? EndDate { get; set; }
    
    [JsonIgnore] public ICollection<Mission> Missions { get; set; } = new List<Mission>();
}