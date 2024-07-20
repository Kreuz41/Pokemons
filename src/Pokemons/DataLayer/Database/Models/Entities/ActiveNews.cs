using System.Text.Json.Serialization;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class ActiveNews
{
    public long Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime AddDate { get; set; }

    [JsonIgnore] public ICollection<News> PlayerNews { get; set; } = [];
}