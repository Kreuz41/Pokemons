using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.PlayerRepos;

public interface IPlayerDatabaseRepository
{
    Task<Player?> GetById(long id);
    Task CreatePlayer(Player player);
    Task UpdatePlayer(Player player);
    Task UpdatePlayers(IEnumerable<Player> players);
}