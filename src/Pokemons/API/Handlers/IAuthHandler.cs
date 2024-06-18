using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;

namespace Pokemons.API.Handlers;

public interface IAuthHandler
{
    Task<CallResult<PlayerAuthResponseDto>> StartSession(long playerId, StartSessionDto dto);
    Task EndSession(long playerId);
}