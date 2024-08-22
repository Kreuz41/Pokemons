using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories.ArActivityRepos
{
    public interface IArActivityRepository
    {
        Task<ArActivity?> GetArActivityByPlayerIdAsync(long playerId);
        Task<ArActivity> CreateArActivityAsync(ArActivity arActivity);
        Task UpdateArActivityAsync(ArActivity arActivity);
        Task DeleteArActivityAsync(long id);
    }
}
