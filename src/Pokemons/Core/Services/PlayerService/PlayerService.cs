using Pokemons.API.Dto.Requests;
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

        player.CurrentEnergy += GetEnergy(player);
        
        var damage = player.DamagePerClick * taps;
        damage = damage > player.CurrentEnergy ? player.CurrentEnergy : damage;
        player.CurrentEnergy -= damage;
        player.LastCommitDamageTime = _timeProvider.Now();
        
        player.Taps += taps;
        player.TotalDamage += damage;

        await _playerRepository.FastUpdate(player);
        
        return (damage, player.DefeatedEntities);
    }

    public async Task<(int, int)> UseSuperCharge(long playerId)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return (0, 0);

        if (!CanUseSuperCharge(player)) return (0, 0);
        
        player.LastSuperChargeActivatedTime = _timeProvider.Now();
        await _playerRepository.FastUpdate(player);
        return (player.SuperCharge, player.DefeatedEntities);
    }

    public async Task<Player?> GetPlayer(long userId) =>
        await _playerRepository.GetPlayerById(userId);

    public async Task<Player> CreatePlayer(long userId, StartSessionDto dto) =>
        await _playerRepository.CreatePlayer(userId, dto);

    public async Task<bool> IsPlayerExist(long playerId) =>
        await _playerRepository.GetPlayerById(playerId) is not null;

    public async Task Save(long playerId) =>
        await _playerRepository.Save(playerId);

    private int GetEnergy(Player player)
    {
        var energy = player.LastCommitDamageTime != default
            ? (int)(_timeProvider.GetSecondsBetweenDateAndNow(player.LastCommitDamageTime) * player.EnergyCharge)
            : player.Energy;
        
        return energy > player.Energy ? player.Energy : energy;
    }

    private bool CanUseSuperCharge(Player player)
    {
        var cooldown = (int)(_timeProvider.Now() - player.LastSuperChargeActivatedTime).TotalHours;
        return cooldown * player.SuperChargeCooldown >= 3;
    }
}