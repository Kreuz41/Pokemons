using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.MissionService;
using Pokemons.Core.Services.PlayerService;

namespace Pokemons.Core.ApiHandlers;

public class MissionHandler : IMissionHandler
{
    public MissionHandler(IMissionService missionService, IPlayerService playerService)
    {
        _missionService = missionService;
        _playerService = playerService;
    }

    private readonly IMissionService _missionService;
    private readonly IPlayerService _playerService;
    
    public async Task<CallResult<MissionsResponseDto>> GetMissions(long playerId)
    {
        if (!await _playerService.IsPlayerExist(playerId)) 
            return CallResult<MissionsResponseDto>.Failure("Player does not exist");

        var result = await _missionService.GetMissions(playerId);
        
        return CallResult<MissionsResponseDto>.Success(new MissionsResponseDto
        {
            Missions = result.Select(m => new MissionInfo(m.Id, m.ActiveMission.IsDifficult, m.CompleteTime))
        });
    }

    public async Task<CallResult<bool>> CompleteMission(long playerId, int missionId)
    {
        if (!await _playerService.IsPlayerExist(playerId)) 
            return CallResult<bool>.Failure("Player does not exist");
        
        if (!await _missionService.IsMissionExist(missionId))
            return CallResult<bool>.Failure("Mission does not exist");

        await _missionService.CompleteMission(playerId, missionId);

        return CallResult<bool>.Success(true);
    }
}