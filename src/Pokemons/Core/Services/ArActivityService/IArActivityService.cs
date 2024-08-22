using Pokemons.API.Dto.Responses;

public interface IArActivityService
{
    /// <summary>
    /// Метод для сбора монетки игроком.
    /// </summary>
    /// <param name="playerId">Идентификатор игрока.</param>
    /// <returns>Кортеж, содержащий успешность операции и данные об активности.</returns>
    Task<(bool Success, ArActivityResponseDto? Activity)> CollectCoinAsync(long playerId);

    /// <summary>
    /// Метод для получения данных об активности игрока.
    /// </summary>
    /// <param name="playerId">Идентификатор игрока.</param>
    /// <returns>DTO с данными об активности.</returns>
    Task<ArActivityResponseDto?> GetArActivityAsync(long playerId);
}
