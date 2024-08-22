using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public WalletRepository(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // Получение кошелька игрока по его ID
        public async Task<Wallet?> GetWalletByPlayerIdAsync(long playerId) =>
            await _context.Wallets
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.PlayerId == playerId);

        // Создание нового кошелька для игрока
        public async Task<Wallet> CreateWalletAsync(Wallet wallet)
        {
            await _unitOfWork.BeginTransaction();
            var createdWallet = await _context.Wallets.AddAsync(wallet);
            await _unitOfWork.CommitTransaction();
            
            return createdWallet.Entity;
        }

        // Обновление кошелька игрока
        public async Task UpdateWalletAsync(Wallet wallet)
        {
            // Отсоединение отслеживаемой сущности, если она есть
            var trackedEntity = _context.ChangeTracker.Entries<Wallet>()
                .FirstOrDefault(e => e.Entity.Id == wallet.Id);
            if (trackedEntity != null)
                _context.Entry(trackedEntity.Entity).State = EntityState.Detached;

            // Прикрепление и изменение состояния сущности
            _context.Attach(wallet);
            _context.Entry(wallet).State = EntityState.Modified;

            await _unitOfWork.BeginTransaction();
            _context.Update(wallet);
            await _unitOfWork.CommitTransaction();

            
        }

        // Удаление кошелька игрока
        public async Task DeleteWalletAsync(long id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallets.Remove(wallet);

                await _unitOfWork.BeginTransaction();
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
        }
    }
}
