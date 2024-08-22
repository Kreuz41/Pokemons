using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.Database.Repositories.ArActivityRepos
{
    public class ArActivityRepository : IArActivityRepository
    {
        public ArActivityRepository(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        // Получение активности игрока по его ID
        public async Task<ArActivity?> GetArActivityByPlayerIdAsync(long playerId) =>
            await _context.ArActivities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.PlayerId == playerId);

        // Создание новой активности для игрока
        public async Task<ArActivity> CreateArActivityAsync(ArActivity arActivity)
        {
            await _unitOfWork.BeginTransaction();
            var createdArActivity = await _context.AddAsync(arActivity);
            await _unitOfWork.CommitTransaction();
            
            return createdArActivity.Entity;
        }

        // Обновление активности игрока
        public async Task UpdateArActivityAsync(ArActivity arActivity)
        {
            // Отсоединение отслеживаемой сущности, если она есть
            var trackedEntity = _context.ChangeTracker.Entries<ArActivity>()
                .FirstOrDefault(e => e.Entity.Id == arActivity.Id);
            if (trackedEntity != null)
                _context.Entry(trackedEntity.Entity).State = EntityState.Detached;

            // Прикрепление и изменение состояния сущности
            _context.Attach(arActivity);
            _context.Entry(arActivity).State = EntityState.Modified;

            await _unitOfWork.BeginTransaction();
            _context.Update(arActivity);
            await _unitOfWork.CommitTransaction();
        }

        // Удаление активности игрока
        public async Task DeleteArActivityAsync(long id)
        {
            var arActivity = await _context.ArActivities.FindAsync(id);
            if (arActivity != null)
            {
                _context.ArActivities.Remove(arActivity);

                await _unitOfWork.BeginTransaction();
                await _context.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
        }
        
    }
}
