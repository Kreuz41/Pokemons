using AutoMapper;
using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;
using Pokemons.API.Handlers;
using Pokemons.Core.Enums.Battles;
using Pokemons.Core.Services.BattleService;
using Pokemons.Core.Services.PlayerService;
using Pokemons.Core.Services.RatingService;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.BattleRepository;
using Pokemons.DataLayer.MasterRepositories.PlayerRepository;

namespace Pokemons.Core.ApiHandlers;

public class BattleHandler : IBattleHandler
{
    public BattleHandler(IBattleService battleService, IMapper mapper, IPlayerService playerService, IRatingService ratingService)
    {
        _battleService = battleService;
        _mapper = mapper;
        _playerService = playerService;
        _ratingService = ratingService;
    }

    private readonly IBattleService _battleService;
    private readonly IPlayerService _playerService;
    private readonly IRatingService _ratingService;
    private readonly IMapper _mapper;

    public async Task<CallResult<CommitDamageResponseDto>> CommitDamage(CommitDamageDto dto, long playerId)
    {
        var userData = await _playerService.CommitDamage(playerId, dto.Taps);
        return await TakeDamage(playerId, userData.Item1, userData.Item2);
    }

    public async Task<CallResult<CommitDamageResponseDto>> UseSuperCharge(long playerId)
    {
        var userData = await _playerService.UseSuperCharge(playerId);
        return await TakeDamage(playerId, userData.Item1, userData.Item2);
    }
    
    private async Task<CallResult<CommitDamageResponseDto>> TakeDamage(long playerId, int damage, int defeatedEntities)
    {
        var battle = await _battleService.TakeDamage(playerId, damage);
        if (battle.Id == 0) return CallResult<CommitDamageResponseDto>.Failure("Invalid battle");
        if (battle.BattleState == BattleState.Defeated)
        {
            battle = await _battleService.CreateNewBattle(playerId, defeatedEntities);
            var defeated = await _playerService.EntityDefeated(playerId);
            await _ratingService.UpdateRating(playerId, defeated);
        }
        
        return CallResult<CommitDamageResponseDto>.Success(_mapper.Map<CommitDamageResponseDto>(battle));
    }
}