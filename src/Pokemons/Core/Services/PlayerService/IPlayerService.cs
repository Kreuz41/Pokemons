namespace Pokemons.Core.Services.PlayerService;

public interface IPlayerService
{
    Task<(int, int)> CommitDamage(long playerId, int taps);
}