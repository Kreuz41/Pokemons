using Pokemons.API.CallResult;
using Pokemons.API.Dto.Requests;
using Pokemons.API.Dto.Responses;
using PokemonsDomain.MessageBroker.Models;

namespace Pokemons.API.Handlers;

public interface IAuthHandler
{
    Task<CallResult<bool>> StartSession(long playerId, EditProfileDto dto);
    Task EndSession(long playerId);
    Task<CallResult<bool>> CreateUser(CreateUserModel data, long playerId);
    Task<CallResult<TapperConfigResponseDto>> GetTapperConfig(long playerId);
    Task<CallResult<ProfileResponseDto>> GetProfile(long playerId);
    Task<CallResult<ProfileResponseDto>> UpdateProfile(long playerId, EditProfileDto dto);
}