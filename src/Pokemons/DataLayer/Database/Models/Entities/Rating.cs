using System.Text.Json.Serialization;
using Pokemons.Core.Enums;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Rating
{
    public long Id { get; set; }
    public long GlobalRatingPosition { get; set; }
    public long LeaguePosition { get; set; }
    public LeagueType LeagueType { get; set; }
    
    public long PlayerId { get; set; }
    [JsonIgnore] public Player Player { get; set; } = null!;
}