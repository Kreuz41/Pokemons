using Pokemons.API.Dto.Responses;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.ArActivityRepos;
using Pokemons.Services;

public class ArActivityService : IArActivityService
{
    private readonly IArActivityRepository _arActivityRepository;
    private readonly IWalletService _walletService;

    private readonly int TRX_TO_ADD_FOR_COIN_COLLECT = 1;
    public ArActivityService(IArActivityRepository arActivityRepository, IWalletService walletService)
    {
        _arActivityRepository = arActivityRepository;
        _walletService = walletService;
    }

    public async Task<(bool Success, ArActivityResponseDto? Activity)> CollectCoinAsync(long playerId)
    {
        var arActivity = await _arActivityRepository.GetArActivityByPlayerIdAsync(playerId);

        if (arActivity == null)
        {
            // Если активность не найдена, создаем её с начальным значением энергии
            arActivity = new ArActivity
            {
                PlayerId = playerId,
                Energy = 1000, // Начальное значение энергии
                TotalCoinsCollected = 0,
                LastCoinCollectedAt = DateTime.UtcNow
            };

            await _arActivityRepository.CreateArActivityAsync(arActivity);
        }
        else
        {
            // Проверка восстановления энергии
            RestoreEnergy(arActivity);
        }

        // Проверка, достаточно ли энергии для сбора монетки
        if (arActivity.Energy < 500)
        {
            return (false, null); // Недостаточно энергии
        }

        // Собираем монетку
        arActivity.Energy -= 500;
        arActivity.TotalCoinsCollected += 1;
        arActivity.LastCoinCollectedAt = DateTime.UtcNow;
        await _walletService.AddTrxToWalletAsync(playerId, TRX_TO_ADD_FOR_COIN_COLLECT);
        // Обновляем информацию об активности в базе данных
        await _arActivityRepository.UpdateArActivityAsync(arActivity);

        // Создаем DTO для ответа
        var activityDto = new ArActivityResponseDto
        {
            Energy = arActivity.Energy,
            TotalCoinsCollected = arActivity.TotalCoinsCollected,
            LastCoinCollectedAt = arActivity.LastCoinCollectedAt
        };

        return (true, activityDto);
    }

    public async Task<ArActivityResponseDto?> GetArActivityAsync(long playerId)
    {
        var arActivity = await _arActivityRepository.GetArActivityByPlayerIdAsync(playerId);

        if (arActivity == null)
        {
            // Если активность не найдена, создаем её с начальным значением энергии
            arActivity = new ArActivity
            {
                PlayerId = playerId,
                Energy = 1000, // Начальное значение энергии
                TotalCoinsCollected = 0,
                LastCoinCollectedAt = DateTime.UtcNow
            };

            await _arActivityRepository.CreateArActivityAsync(arActivity);
        }

        // Проверка восстановления энергии
        RestoreEnergy(arActivity);

        // Создаем DTO для ответа
        return new ArActivityResponseDto
        {
            Energy = arActivity.Energy,
            TotalCoinsCollected = arActivity.TotalCoinsCollected,
            LastCoinCollectedAt = arActivity.LastCoinCollectedAt
        };
    }

    private void RestoreEnergy(ArActivity arActivity)
    {
        var now = DateTime.UtcNow;
        var recoveryRatePerDay = 1000; // Энергия за 24 часа
        var recoveryInterval = TimeSpan.FromDays(1);

        // Определяем сколько времени прошло с последнего сбора монетки
        var timeSinceLastCollect = now - arActivity.LastCoinCollectedAt;

        // Определяем сколько энергии должно быть восстановлено
        var energyRecovered = (int)(timeSinceLastCollect.TotalSeconds / recoveryInterval.TotalSeconds * recoveryRatePerDay);

        // Обновляем значение энергии, не превышая максимального значения
        arActivity.Energy = Math.Min(arActivity.Energy + energyRecovered, recoveryRatePerDay);

        // Обновляем дату последнего восстановления
        arActivity.LastCoinCollectedAt = now;
    }
}
