﻿using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.GuildService;
using Pokemons.Core.Services.MarketService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.Core.Services.RatingService;
using Pokemons.DataLayer.MasterRepositories.CommonRepository;
using Pokemons.DataLayer.MasterRepositories.NotificationRepository;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.Core.ApiHandlers;

public class AuthHandler : IAuthHandler
{
    public AuthHandler(IMapper mapper, IPlayerService playerService, 
        IBattleService battleService, IMarketService marketService, IRatingService ratingService, 
        IGuildService guildService, ICommonRepository commonRepository, INotificationRepository notificationRepository)
    {
        _mapper = mapper;
        _playerService = playerService;
        _battleService = battleService;
        _marketService = marketService;
        _ratingService = ratingService;
        _guildService = guildService;
        _commonRepository = commonRepository;
        _notificationRepository = notificationRepository;
    }
    
    private readonly IPlayerService _playerService;
    private readonly IBattleService _battleService;
    private readonly IMarketService _marketService;
    private readonly IRatingService _ratingService;
    private readonly IGuildService _guildService;
    private readonly ICommonRepository _commonRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public async Task<CallResult<bool>> StartSession(long playerId, EditProfileDto dto)
    {
        if (!await _playerService.IsPlayerExist(playerId))
            return CallResult<bool>.Failure("Player does not exist");

        var player = await _playerService.GetPlayer(playerId);
        
        return CallResult<bool>.Success(true);
    }

    public async Task EndSession(long playerId)
    {
        await _guildService.Save(playerId);
        await _battleService.Save(playerId);
        await _marketService.Save(playerId);
        await _ratingService.Save(playerId);
        
        await _playerService.Save(playerId);
    }

    public async Task<CallResult<bool>> CreateUser(CreateUserModel data, long playerId)
    {
        if (await _playerService.IsPlayerExist(playerId)) 
            return CallResult<bool>.Failure("Player already exist");

        await _commonRepository.CreateUser(data, playerId);

        return CallResult<bool>.Success(true);
    }

    public async Task<CallResult<TapperConfigResponseDto>> GetTapperConfig(long playerId)
    {
        if (!await _playerService.IsPlayerExist(playerId)) 
            return CallResult<TapperConfigResponseDto>.Failure("Player does not exist");

        var player = await _playerService.GetPlayer(playerId);
        var battle = await _battleService.GetBattleByPlayerId(playerId);

        if (player is null || battle is null)
            return CallResult<TapperConfigResponseDto>.Failure("Battle not found");

        var result = _mapper.Map<TapperConfigResponseDto>(player);
        result.EntityData = _mapper.Map<CommitDamageResponseDto>(battle);
        result.EntityData.RemainingEnergy = player!.CurrentEnergy;
        result.SuperChargeRemaining = await _playerService.GetSuperChargeSecondsRemaining(playerId);

        return CallResult<TapperConfigResponseDto>.Success(result);
    }

    public async Task<CallResult<ProfileResponseDto>> GetProfile(long playerId)
    {
        var player = await _playerService.GetPlayer(playerId);
        if (player is null) return CallResult<ProfileResponseDto>.Failure("Player does not exist");
        
        var response = new ProfileResponseDto
        {
            DefeatedEntities = player.DefeatedEntities,
            DamagePerClick = player.DamagePerClick,
            EnergyCooldown = player.EnergyCharge,
            TotalTaps = player.Taps,
            TotalDamage = player.TotalDamage,
            SuperChargeCooldown = player.SuperChargeCooldown,
            PhotoUrl = player.PhotoUrl,
            Exp = player.Exp,
            Level = player.Level,
            IsPremium = player.IsPremium,
            CryptoBalance = player.CryptoBalance,
            Firstname = player.Name,
            Lastname = player.Surname,
            UnreadNews = await _notificationRepository.GetUnreadNewsCount(playerId),
            UnreadNotify = await _notificationRepository.GetUnreadNotifications(playerId)
        };

        return CallResult<ProfileResponseDto>.Success(response);
    }

    public async Task<CallResult<ProfileResponseDto>> UpdateProfile(long playerId, EditProfileDto dto)
    {
        var player = await _playerService.GetPlayer(playerId);
        if (player is null) return CallResult<ProfileResponseDto>.Failure("Player not found");
        
        await _playerService.UpdatePlayerData(dto, player);

        return await GetProfile(playerId);
    }
}