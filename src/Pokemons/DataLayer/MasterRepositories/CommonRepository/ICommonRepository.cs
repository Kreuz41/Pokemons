using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.DataLayer.MasterRepositories.CommonRepository;

public interface ICommonRepository
{
    Task CreateUser(CreateUserModel userModel, long playerId);
}