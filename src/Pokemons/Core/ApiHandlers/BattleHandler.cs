using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Enums.Battles;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;

namespace Pokemons.Core.ApiHandlers;

public class BattleHandler : IBattleHandler
{
    public BattleHandler(IBattleService battleService, IMapper mapper, IPlayerService playerService)
    {
        _battleService = battleService;
        _mapper = mapper;
        _playerService = playerService;
    }

    private readonly IBattleService _battleService;
    private readonly IPlayerService _playerService;
    private readonly IMapper _mapper;

    public async Task<CallResult<CommitDamageResponseDto>> CommitDamage(CommitDamageDto dto, long playerId)
    {
        var damage = await _playerService.CommitDamage(playerId, dto.Taps);
        var battle = await _battleService.TakeDamage(playerId, damage.Item1);

        if (battle.Id == 0) return CallResult<CommitDamageResponseDto>.Failure("Invalid battle");
        if (battle.BattleState == BattleState.Defeated)
            battle = await _battleService.CreateNewBattle(playerId, damage.Item2);
        
        return CallResult<CommitDamageResponseDto>.Success(_mapper.Map<CommitDamageResponseDto>(battle));
    }
}