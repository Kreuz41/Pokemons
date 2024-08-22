using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.Database.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet?> GetWalletByPlayerIdAsync(long playerId);
        Task<Wallet> CreateWalletAsync(Wallet wallet);
        Task UpdateWalletAsync(Wallet wallet);
        Task DeleteWalletAsync(long id);
    }
}
