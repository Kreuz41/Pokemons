using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.MissionRepos;

namespace Pokemons.DataLayer.MasterRepositories.MissionRepository;

public class MissionRepository : IMissionRepository
{
    public MissionRepository(IMissionDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    private readonly IMissionDatabaseRepository _databaseRepository;

    public async Task<IEnumerable<Mission>> GetAllMissions(long playerId) =>
        await _databaseRepository.GetAllMissions(playerId);

    public async Task<Mission?> GetMission(int missionId, long playerId) =>
        await _databaseRepository.GetMission(missionId, playerId);

    public async Task UpdateMission(Mission mission) =>
        await _databaseRepository.UpdateMission(mission);

    public async Task<IEnumerable<ActiveMission>> GetActiveMissions() =>
        await _databaseRepository.GetActiveMissions();

    public async Task SaveMissions(IEnumerable<Mission> missions) =>
        await _databaseRepository.SaveMissions(missions);
}