namespace Pokemons.API.Dto.Requests;

public class StartSessionDto
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public required string Hash { get; set; }
    public string? PhotoUrl { get; set; }
}