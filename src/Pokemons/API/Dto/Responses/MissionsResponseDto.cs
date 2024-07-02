namespace Pokemons.API.Dto.Responses;

public class MissionsResponseDto
{
    public IEnumerable<MissionInfo> Missions { get; set; } = [];
}

public record MissionInfo(long Id, int MainId, bool IsDifficult, bool IsActive);