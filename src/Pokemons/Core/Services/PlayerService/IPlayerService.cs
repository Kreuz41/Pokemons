using Pokemons.API.Dto.Requests;
using Pokemons.DataLayer.Database.Models.Entities;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.Core.Services.PlayerService;

public interface IPlayerService
{
    Task<(int, int)> CommitDamage(long playerId, int taps);
    Task<Player?> GetPlayer(long userId);
    Task<Player> CreatePlayer(long userId, CreateUserModel dto);
    Task<bool> IsPlayerExist(long playerId);
    Task<(int, int)> UseSuperCharge(long playerId);
    Task Save(long playerId);
    Task<int> EntityDefeated(long playerId);
    Task UpdatePlayerData(StartSessionDto dto, Player player);
    Task Update(Player player);
    Task ConfirmMissionReward(long playerId, int activeMissionReward);
}