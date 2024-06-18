using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;

namespace Pokemons.API.Handlers;

public interface IAuthHandler
{
    Task<CallResult<PlayerAuthResponseDto>> StartSession(long userId, StartSessionDto dto);
    Task EndSession(long playerId);
}