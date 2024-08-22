using Pokemons.API.Dto.Responses;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Services
{
    public interface IWalletService
    {
        Task<WalletDto> GetWalletByPlayerIdAsync(long playerId);
        Task<WalletDto> CreateWalletAsync(Wallet wallet);
        Task UpdateWalletAsync(Wallet wallet);
        Task DeleteWalletAsync(long id);
        Task<bool> WithdrawTrxAsync(long playerId, int amount);
        Task AddTrxToWalletAsync(long playerId, int amount);
        Task DepositUsdtToWalletAsync(long playerId, decimal amount);
        Task<string> CreateInvoiceAsync(long playerId ,decimal amount);
    }
}
