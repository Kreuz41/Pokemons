using Pokemons.API.Dto.Requests;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;
using Pokemons.DataLayer.MasterRepositories.ReferralNodeRepository;
using Pokemons.Services;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.Core.Services.PlayerService;

public class PlayerService : IPlayerService
{
    public PlayerService(IPlayerRepository playerRepository, ITimeProvider timeProvider, 
        IBattleRepository battleRepository, IReferralNodeRepository referralRepository, IWalletService walletService)
    {
        _playerRepository = playerRepository;
        _timeProvider = timeProvider;
        _battleRepository = battleRepository;
        _referralRepository = referralRepository;
        _walletService = walletService;
    }

    private readonly IPlayerRepository _playerRepository;
    private readonly IBattleRepository _battleRepository;
    private readonly IReferralNodeRepository _referralRepository;
    private readonly ITimeProvider _timeProvider;
    private readonly IWalletService _walletService;
    
    private const int PremCost = 10;

    public async Task<(int, int)> CommitDamage(long playerId, int taps)
    {
        var battle = await _battleRepository.GetPlayerBattle(playerId);
        var player = await GetPlayer(playerId);
        if (player is null || battle is null) return (0, 0);

        var actualTaps = taps > player.CurrentEnergy 
                        ? player.CurrentEnergy
                        : taps;

        var damage = actualTaps * player.DamagePerClick;

        player.CurrentEnergy -= actualTaps;

        player.LastCommitDamageTime = _timeProvider.Now();

        if (player.IsFirstEntry)
            player.IsFirstEntry = false;
        
        player.Taps += actualTaps;
        player.TotalDamage += damage;

        if (battle.IsGold)
            player.GoldBalance += damage;
        else
            await TopUpPlayerBalance(player, damage);

        await _playerRepository.FastUpdate(player);
        
        return (damage, player.DefeatedEntities);
    }

    public async Task TopUpPlayerBalance(Player player, int value)
    {
        player.Balance += value;

        var parents = await _referralRepository.GetParentsForPlayer(player.Id);
        foreach (var node in parents)
        {
            node.BalanceValue += value;
            var parent = await _playerRepository.GetPlayerById(node.ReferrerId);
            if (parent is null) continue;
            
            parent.Balance += value;
            await _playerRepository.Update(parent);
        }

        await _referralRepository.UpdateEnumerable(parents);
    }

    public async Task<(int, int)> UseSuperCharge(long playerId)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return (0, 0);

        if (!CanUseSuperCharge(player)) return (0, 0);
        var superChargeDamage = player.SuperCharge * player.DamagePerClick;

        var battle = await _battleRepository.GetPlayerBattle(playerId);

        if (battle is null) return (0, 0);

        var actualDamage = (int)(battle.RemainingHealth - superChargeDamage > 0 
        ? superChargeDamage
        : battle.RemainingHealth);

        if (battle.IsGold)
            player.GoldBalance += actualDamage;
        else
            await TopUpPlayerBalance(player, actualDamage);

        player.LastSuperChargeActivatedTime = _timeProvider.Now();
        await _playerRepository.FastUpdate(player);
        return (superChargeDamage, player.DefeatedEntities);
    }

    public async Task<Player?> GetPlayer(long userId)
    {
        var player = await _playerRepository.GetPlayerById(userId);
        if (player is null) return null;
        
        var energy = GetEnergy(player);
        player.CurrentEnergy += energy;
        player.CurrentEnergy = player.CurrentEnergy > player.Energy ? player.Energy : player.CurrentEnergy;
        
        return player;
    }

    public async Task<Player> CreatePlayer(long userId, CreateUserModel dto) =>
        await _playerRepository.CreatePlayer(userId, dto);

    public async Task<bool> IsPlayerExist(long playerId) =>
        await _playerRepository.GetPlayerById(playerId) is not null;

    public async Task Save(long playerId) =>
        await _playerRepository.Save(playerId);

    public async Task<int> EntityDefeated(long playerId)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return 0;
        player.DefeatedEntities++;
        LevelUpdate(player);
        await _playerRepository.FastUpdate(player);
        return player.DefeatedEntities;
    }

    public async Task UpdatePlayerData(EditProfileDto dto, Player player)
    {
        player.Name = dto.Name;
        player.Surname = dto.Surname;
        player.Username = dto.Username;
        
        await _playerRepository.FastUpdate(player);
    }

    public async Task Update(Player player) =>
        await _playerRepository.Update(player);

    public async Task ConfirmMissionReward(long playerId, int activeMissionReward)
    {
        var player = await _playerRepository.GetPlayerById(playerId);
        if (player is null) return;
        
        player.Balance += activeMissionReward;
        await _playerRepository.Update(player);
    }

    public async Task<bool> IsEnoughCrypto(long playerId)
    {
        var wallet = await _walletService.GetWalletByPlayerIdAsync(playerId);

        return wallet?.Usdt >= PremCost;
    }

    public async Task BuyPrem(long playerId)
    {
        var wallet = await _walletService.GetWalletByPlayerIdAsync(playerId);
        var player = await GetPlayer(playerId);
        if (wallet is null || wallet?.Usdt < PremCost || player!.IsPremium)
            return;
        
        await _walletService.DepositUsdtToWalletAsync(playerId, -PremCost);
        
        player.IsPremium = true;
        
        await _playerRepository.FastUpdate(player);
    }

    public async Task<DateTime> GetSuperChargeSecondsRemaining(long playerId)
    {
        var player = await GetPlayer(playerId);
        if (player is null) return DateTime.Now;

        var cooldown =
            (int)(_timeProvider.Now() - player.LastSuperChargeActivatedTime).TotalSeconds;
        
        return DateTime.Now + TimeSpan.FromHours((double)player.SuperChargeCooldown)
            .Subtract(TimeSpan.FromSeconds(cooldown));
    }

    private void LevelUpdate(Player player)
    {
        player.Exp += 5000;
        if (player.Exp != 50000) return;
        
        player.Exp = 0;
        player.Level++;
    }

    private int GetEnergy(Player player)
    {
        var energy = player.LastCommitDamageTime != default
            ? (int)(_timeProvider.GetSecondsBetweenDateAndNow(player.LastCommitDamageTime) / player.EnergyCharge)
            : player.Energy;

        if (energy < 0) energy = 0;
        
        return energy > player.Energy ? player.Energy : energy;
    }

    private bool CanUseSuperCharge(Player player)
    {
        var waitTimeInHours = (_timeProvider.Now() - player.LastSuperChargeActivatedTime).TotalHours;
        return waitTimeInHours >= (double)player.SuperChargeCooldown;
    }
}