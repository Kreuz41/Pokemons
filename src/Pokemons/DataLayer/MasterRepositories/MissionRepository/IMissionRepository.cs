using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.MissionRepository;

public interface IMissionRepository
{
    Task<IEnumerable<Mission>> GetAllMissions(long playerId);
    Task<Mission?> GetMission(int missionId);
    Task UpdateMission(Mission mission);
    Task<IEnumerable<ActiveMission>> GetActiveMissions();
    Task SaveMissions(IEnumerable<Mission> missions);
}