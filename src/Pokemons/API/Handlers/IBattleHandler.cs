using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;

namespace Pokemons.API.Handlers;

public interface IBattleHandler
{
    Task<CallResult<CommitDamageResponseDto>> CommitDamage(CommitDamageDto dto, long playerId);
}