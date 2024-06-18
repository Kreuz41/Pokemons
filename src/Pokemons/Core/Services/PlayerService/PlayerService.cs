using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;

namespace Pokemons.Core.Services.PlayerService;

public class PlayerService : IPlayerService
{
    public PlayerService(IPlayerRepository playerRepository, ITimeProvider timeProvider)
    {
        _playerRepository = playerRepository;
        _timeProvider = timeProvider;
    }

    private readonly IPlayerRepository _playerRepository;
    private readonly ITimeProvider _timeProvider;

    public async Task<(int, int)> CommitDamage(long playerId, int taps)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return (0, 0);

        var energy = GetEnergy(player);
        
        var damage = player.DamagePerClick * taps;
        damage = damage > energy ? energy : damage;
        player.LastCommitDamageTime = _timeProvider.Now();
        
        player.Taps += taps;
        player.TotalDamage += damage;

        await _playerRepository.FastUpdate(player);
        
        return (damage, player.DefeatedEntities);
    }

    private int GetEnergy(Player player)
    {
        var energy = player.LastCommitDamageTime != default
            ? (int)(_timeProvider.GetSecondsBetweenDateAndNow(player.LastCommitDamageTime) * player.EnergyCharge)
            : player.Energy;
        
        return energy > player.Energy ? player.Energy : energy;
    }
}