using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.MarketService;
using Pokemons.Core.Services.PlayerService;

namespace Pokemons.Core.ApiHandlers;

public class AuthHandler : IAuthHandler
{
    public AuthHandler(IMapper mapper, IPlayerService playerService, 
        IBattleService battleService, IMarketService marketService)
    {
        _mapper = mapper;
        _playerService = playerService;
        _battleService = battleService;
        _marketService = marketService;
    }
    
    private readonly IPlayerService _playerService;
    private readonly IBattleService _battleService;
    private readonly IMarketService _marketService;
    private readonly IMapper _mapper;

    public async Task<CallResult<PlayerAuthResponseDto>> StartSession(long playerId, StartSessionDto dto)
    {
        if (!await _playerService.IsPlayerExist(playerId))
            await CreatePlayer(playerId, dto);

        var player = await _playerService.GetPlayer(playerId);
        var battle = await _battleService.GetBattleByPlayerId(playerId);

        var result = _mapper.Map<PlayerAuthResponseDto>(player);
        result.EntityData = _mapper.Map<CommitDamageResponseDto>(battle);
        return CallResult<PlayerAuthResponseDto>.Success(result);
    }

    private async Task CreatePlayer(long playerId, StartSessionDto dto)
    {
        await _playerService.CreatePlayer(playerId, dto);
        await _battleService.CreateNewBattle(playerId, 0);
        await _marketService.CreateMarket(playerId);
    }

    public async Task EndSession(long playerId)
    {
        await _playerService.Save(playerId);
        await _marketService.Save(playerId);
        await _playerService.Save(playerId);
    }
}