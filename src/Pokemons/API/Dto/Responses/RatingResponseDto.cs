namespace Pokemons.API.Dto.Responses;

public class RatingResponseDto
{
    public IEnumerable<RatingResponse> Ratings { get; set; } = [];
}

public record RatingResponse(string FirstName, string Surname, string PhotoUrl, long Position, int EntitiesCount);