using System.Reflection;
using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Enums;
using Pokemons.Core.Services.MarketService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.MarketRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;

namespace Pokemons.Core.ApiHandlers;

public class MarketHandler : IMarketHandler
{
    public MarketHandler(IMapper mapper, IMarketService marketService, IPlayerService playerService)
    {
        _marketService = marketService;
        _playerService = playerService;
        _mapper = mapper;
    }
    
    private readonly IMarketService _marketService;
    private readonly IPlayerService _playerService;
    private readonly IMapper _mapper;
    
    public async Task<CallResult<MarketResponseDto>> GetMarketByUserId(long playerId)
    {
        var market = await _marketService.GetMarketByPlayerId(playerId);
        var response = _mapper.Map<MarketResponseDto>(market);
        return CallResult<MarketResponseDto>.Success(response);
    }

    public async Task<CallResult<MarketResponseDto>> UpgradeUserStat(long playerId, StatType type)
    {
        var player = await _playerService.GetPlayer(playerId);
        if (player is null) return CallResult<MarketResponseDto>.Failure("Player does not exist");

        var market = await _marketService.GetMarketByPlayerId(playerId);
        if (market is null)
            return CallResult<MarketResponseDto>.Failure("Market not found");

        if (!_marketService.TryUpgradeStat(player, market, type))
            return CallResult<MarketResponseDto>.Failure("Not enough money or invalid stat type");

        await _playerService.Update(player);
        await _marketService.Update(market);

        return CallResult<MarketResponseDto>.Success(_mapper.Map<MarketResponseDto>(market));
    }
}