using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;

namespace Pokemons.API.Handlers;

public interface IAuthHandler
{
    Task<CallResult<PlayerAuthResponseDto>> StartSession(long playerId, StartSessionDto dto);
    Task EndSession(long playerId);
    Task<CallResult<bool>> CreateUser(CreatePlayerDto data, long playerId);
    Task<CallResult<TapperConfigResponseDto>> GetTapperConfig(long playerId);
    Task<CallResult<ProfileResponseDto>> GetProfile(long playerId);
}