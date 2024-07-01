using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.MissionService;

public interface IMissionService
{
    Task<IEnumerable<Mission>> GetMissions(long playerId);
    Task<bool> IsMissionExist(int missionId);
    Task CompleteMission(long playerId, int missionId);
    Task CreateMissions(long playerId);
}