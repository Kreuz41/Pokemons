using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.MissionRepository;

namespace Pokemons.Core.Services.MissionService;

public class MissionService : IMissionService
{
    public MissionService(IMissionRepository missionRepository, ITimeProvider timeProvider)
    {
        _missionRepository = missionRepository;
        _timeProvider = timeProvider;
    }

    private readonly IMissionRepository _missionRepository;
    private readonly ITimeProvider _timeProvider;

    public async Task<IEnumerable<Mission>> GetMissions(long playerId) =>
        await _missionRepository.GetAllMissions(playerId);

    public async Task<bool> IsMissionExist(int missionId, long playerId) =>
        await _missionRepository.GetMission(missionId, playerId) != null;

    public async Task<Mission?> CompleteMission(long playerId, int missionId)
    {
        var mission = await _missionRepository.GetMission(missionId, playerId);
        if (mission is null) return null;
        
        mission.CompleteTime = _timeProvider.Now();
        await _missionRepository.UpdateMission(mission);

        return mission;
    }

    public async Task CreateMissions(long playerId)
    {
        var active = await _missionRepository.GetActiveMissions();
        var missions = active.Select(a => new Mission
        {
            ActiveMissionId = a.Id,
            PlayerId = playerId
        });
        await _missionRepository.SaveMissions(missions);
    }

    public async Task<bool> IsMissionCompleted(int missionId, long playerId)
    {
        var mission = await _missionRepository.GetMission(missionId, playerId);
        return mission?.CompleteTime is not null;
    }
}