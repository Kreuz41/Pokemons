namespace Pokemons.API.Dto.Requests;

public class CreatePlayerDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Username { get; set; }
    public required string Hash { get; set; }
    public string? PhotoUrl { get; set; }
    public long? RefId { get; set; }
    public long Balance { get; set; }
}