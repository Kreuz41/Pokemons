using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.MissionRepos;

public interface IMissionDatabaseRepository
{
    Task<IEnumerable<Mission>> GetAllMissions(long playerId);
    Task<Mission?> GetMission(int missionId, long playerId);
    Task UpdateMission(Mission mission);
    Task<IEnumerable<ActiveMission>> GetActiveMissions();
    Task SaveMissions(IEnumerable<Mission> missions);
}