using Pokemons.API.Dto.Requests;
using Pokemons.DataLayer.Database.Models.Entities;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.DataLayer.MasterRepositories.PlayerRepository;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerById(long id);
    Task<Player> CreatePlayer(long userId, CreateUserModel dto);
    Task Save(long id);
    Task Update(Player player);
    Task FastUpdate(Player player);
}