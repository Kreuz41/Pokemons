using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.MissionService;

public interface IMissionService
{
    Task<IEnumerable<Mission>> GetMissions(long playerId);
    Task<bool> IsMissionExist(int missionId, long playerId);
    Task<Mission?> CompleteMission(long playerId, int missionId);
    Task CreateMissions(long playerId);
    Task<bool> IsMissionCompleted(int missionId, long playerId);
}