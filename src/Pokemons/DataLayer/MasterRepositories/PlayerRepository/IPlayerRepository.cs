using Pokemons.API.Dto.Requests;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.PlayerRepository;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerById(long id);
    Task<Player> CreatePlayer(long userId, StartSessionDto dto);
    Task Save(long id);
    Task Update(Player player);
    Task FastUpdate(Player player);
}