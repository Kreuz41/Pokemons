using PokemonsDomain.Notification;

namespace Pokemons.DataLayer.Database.Models.Entities;

public class Notification
{
    public long Id { get; set; }
    public bool IsRead { get; set; } = false;

    public NotificationType NotificationType { get; set; }
    public string? ReferralName { get; set; }
    public string? GuildMemberName { get; set; }
    public decimal? TopUpValue { get; set; }
    public long PlayerId { get; set; }
    public Player Player { get; set; } = null!;
}