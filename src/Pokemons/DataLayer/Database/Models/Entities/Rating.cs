using Pokemons.Core.Enums;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Rating
{
    public long Id { get; set; }
    public long GlobalRatingPosition { get; set; }
    public long LeaguePosition { get; set; }
    public LeagueType LeagueType { get; set; }
    
    public long PlayerId { get; set; }
    public Player Player { get; set; } = null!;
}