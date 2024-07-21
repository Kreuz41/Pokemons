namespace PokemonsDomain.Notification;

public class NotifyDto
{
    public NotificationType NotificationType { get; set; }
    public string? ReferralName { get; set; }
    public string? GuildMemberName { get; set; }
    public decimal? TopUpValue { get; set; }
    public long PlayerId { get; set; }
}