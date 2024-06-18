using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Enums.Battles;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;
using Pokemons.DataLayer.MasterRepositories.MarketRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;

namespace Pokemons.Core.ApiHandlers;

public class AuthHandler : IAuthHandler
{
    public AuthHandler(IPlayerRepository playerRepository, IBattleRepository battleRepository, 
        IMarketRepository marketRepository, IMapper mapper)
    {
        _playerRepository = playerRepository;
        _battleRepository = battleRepository;
        _marketRepository = marketRepository;
        _mapper = mapper;
    }

    private readonly IPlayerRepository _playerRepository;
    private readonly IBattleRepository _battleRepository;
    private readonly IMarketRepository _marketRepository;
    private readonly IPlayerService _playerService;
    private readonly IBattleService _battleService;
    private readonly IMapper _mapper;

    public async Task<CallResult<PlayerAuthResponseDto>> StartSession(long userId, StartSessionDto dto)
    {
        var player = await _playerRepository.GetPlayerById(userId);
        if (player is not null)
        {
            var battle = await _battleRepository.GetPlayerBattle(userId);
            return CallResult<PlayerAuthResponseDto>.Success(new PlayerAuthResponseDto
            {
                Defeated = player.DefeatedEntities,
                Balance = player.Balance,
                DamagePerClick = player.DamagePerClick,
                EntityData = _mapper.Map<CommitDamageResponseDto>(battle)
            });
        }

        player = await _playerRepository.CreatePlayer(userId, dto);
        await _marketRepository.CreateMarket(userId);
        // var initBattle = await _battleRepository.CreateBattle(player.Id, player.DefeatedEntities);
        
        return CallResult<PlayerAuthResponseDto>.Success(new PlayerAuthResponseDto
        {
            Defeated = player.DefeatedEntities,
            Balance = player.Balance,
            DamagePerClick = player.DamagePerClick,
            EntityData = _mapper.Map<CommitDamageResponseDto>(20)
        });
    }

    public async Task EndSession(long playerId)
    {
        await _marketRepository.Save(playerId);
        await _playerRepository.Save(playerId);
    }
}