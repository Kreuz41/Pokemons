using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.MarketService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.Core.Services.RatingService;
using Pokemons.Core.Services.ReferralService;

namespace Pokemons.Core.ApiHandlers;

public class AuthHandler : IAuthHandler
{
    public AuthHandler(IMapper mapper, IPlayerService playerService, 
        IBattleService battleService, IMarketService marketService, 
        IReferralService referralService, IRatingService ratingService)
    {
        _mapper = mapper;
        _playerService = playerService;
        _battleService = battleService;
        _marketService = marketService;
        _referralService = referralService;
        _ratingService = ratingService;
    }
    
    private readonly IPlayerService _playerService;
    private readonly IBattleService _battleService;
    private readonly IMarketService _marketService;
    private readonly IRatingService _ratingService;
    private readonly IReferralService _referralService;
    private readonly IMapper _mapper;

    public async Task<CallResult<PlayerAuthResponseDto>> StartSession(long playerId, StartSessionDto dto)
    {
        if (!await _playerService.IsPlayerExist(playerId))
            return CallResult<PlayerAuthResponseDto>.Failure("Player does not exist");

        var player = await _playerService.GetPlayer(playerId);
        var battle = await _battleService.GetBattleByPlayerId(playerId);

        var result = _mapper.Map<PlayerAuthResponseDto>(player);
        result.EntityData = _mapper.Map<CommitDamageResponseDto>(battle);
        return CallResult<PlayerAuthResponseDto>.Success(result);
    }

    public async Task EndSession(long playerId)
    {
        await _playerService.Save(playerId);
        await _battleService.Save(playerId);
        await _marketService.Save(playerId);
        await _ratingService.Save(playerId);
    }

    public async Task<CallResult<bool>> CreateUser(StartSessionDto data, long playerId)
    {
        if (await _playerService.IsPlayerExist(playerId)) return CallResult<bool>.Failure("Player already exist");

        await CreatePlayer(playerId, data);

        return CallResult<bool>.Success(true);
    }

    private async Task CreatePlayer(long playerId, StartSessionDto dto)
    {
        await _playerService.CreatePlayer(playerId, dto);
        await _battleService.CreateNewBattle(playerId, 0);
        await _marketService.CreateMarket(playerId);
        await _ratingService.CreateRating(playerId);
        
        if (dto.RefId is not null)
            await _referralService.CreateNode(playerId, dto.RefId.Value);
    }
}