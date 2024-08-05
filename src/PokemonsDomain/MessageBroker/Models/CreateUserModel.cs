namespace PokemonsDomain.MessageBroker.Models;

public class CreateUserModel
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Username { get; set; }
    public long UserId { get; set; }
    public required string Hash { get; set; }
    public string? PhotoUrl { get; set; }
    public long? RefId { get; set; }
    public string? LangCode { get; set; }
}