using System.Text;
using System.Text.Json;
using Pokemons.API.Dto.Responses;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories;
using Pokemons.Core.BackgroundServices.NotificationCreator;
using PokemonsDomain.Notification;

namespace Pokemons.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly HttpClient _httpClient;

        public WalletService(IWalletRepository walletRepository, HttpClient httpClient)
        {
            _walletRepository = walletRepository;
            _httpClient = httpClient;
        }


        public async Task<WalletDto> GetWalletByPlayerIdAsync(long playerId)
        {
           var wallet = await _walletRepository.GetWalletByPlayerIdAsync(playerId);

            if (wallet is null){
               return await CreateWalletAsync(new Wallet{
                    PlayerId = playerId,    
                });
            }

            return new WalletDto{
                Trx = wallet.TRXBalance,
                Usdt = wallet.USDTBalance
            };
        }

        public async Task<WalletDto> CreateWalletAsync(Wallet wallet)
        {
            wallet = await _walletRepository.CreateWalletAsync(wallet);

            return new WalletDto{
                Trx = wallet.TRXBalance,
                Usdt = wallet.USDTBalance
            };
        }

        public async Task UpdateWalletAsync(Wallet wallet)
        {
             await _walletRepository.UpdateWalletAsync(wallet);
        }

        public async Task DeleteWalletAsync(long id)
        {
            await _walletRepository.DeleteWalletAsync(id);
        }
         public async Task AddTrxToWalletAsync(long playerId, int amount)
    {
        var wallet = await _walletRepository.GetWalletByPlayerIdAsync(playerId);

        if (wallet == null)
        {
            wallet = new Wallet
            {
                PlayerId = playerId,
                TRXBalance = 1 
            };
            await _walletRepository.CreateWalletAsync(wallet);
        }
        else
        {
            wallet.TRXBalance += amount;
            await _walletRepository.UpdateWalletAsync(wallet);
        }
    }
     public async Task<bool> WithdrawTrxAsync(long playerId, int amount)
    {
        var wallet = await _walletRepository.GetWalletByPlayerIdAsync(playerId);

        if (wallet == null || wallet.TRXBalance < amount)
        {
            return false; // Кошелек не найден или недостаточно средств
        }

        wallet.TRXBalance -= amount;
        await _walletRepository.UpdateWalletAsync(wallet);
        return true;
    }
     public async Task DepositUsdtToWalletAsync(long playerId, decimal amount)
        {
            var wallet = await _walletRepository.GetWalletByPlayerIdAsync(playerId);
            if (wallet != null)
            {
                wallet.USDTBalance += amount; // Добавляем сумму к USDT балансу
                await _walletRepository.UpdateWalletAsync(wallet);
                NotificationCreator.AddNotification(new Notification{
            PlayerId = playerId,
            NotificationType = NotificationType.TopUp,
            TopUpValue = amount,
                }
                
                );
            }


            
        }
        public async Task<string> CreateInvoiceAsync(long playerId ,decimal amount)
    {
        var requestBody = new
        {
            amount,
            payload = new {
                playerId
            },
            webhook = "https://cixtaptest.ru/api/wallet/depositUsdt",
            type = "TRC-20 USDT",
        };

        var requestContent = new StringContent(
            JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://cixtap.com/api/v1/invoices/createInvoice", requestContent);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonSerializer.Deserialize<JsonDocument>(responseBody);

        return jsonResponse.RootElement.GetProperty("pay_url").GetString();
    }
    
    }
    
}
