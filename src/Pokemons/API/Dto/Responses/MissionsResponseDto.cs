namespace Pokemons.API.Dto.Responses;

public class MissionsResponseDto
{
    public IEnumerable<MissionInfo> Missions { get; set; } = [];
}

public record MissionInfo(long Id, bool IsDifficult, bool IsActive, int ActiveMissionReward);