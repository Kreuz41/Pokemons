using Pokemons.API.Dto.Requests;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.PlayerService;

public interface IPlayerService
{
    Task<(int, int)> CommitDamage(long playerId, int taps);
    Task<Player?> GetPlayer(long userId);
    Task<Player> CreatePlayer(long userId, CreatePlayerDto dto);
    Task<bool> IsPlayerExist(long playerId);
    Task<(int, int)> UseSuperCharge(long playerId);
    Task Save(long playerId);
    Task<int> EntityDefeated(long playerId);
    Task UpdatePlayerData(StartSessionDto dto, Player player);
    Task Update(Player player);
}