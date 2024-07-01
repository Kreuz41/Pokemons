using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;

namespace Pokemons.API.Handlers;

public interface IMissionHandler
{
    Task<CallResult<MissionsResponseDto>> GetMissions(long playerId);
    Task<CallResult<bool>> CompleteMission(long playerId, int missionId);
}