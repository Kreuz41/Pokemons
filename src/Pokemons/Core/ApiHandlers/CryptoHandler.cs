using Pokemons.API.CallResult;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.PlayerService;

namespace Pokemons.Core.ApiHandlers;

public class CryptoHandler : ICryptoHandler
{
    public CryptoHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    private readonly IPlayerService _playerService;

    public async Task<CallResult<bool>> BuyPrem(long playerId)
    {
        if (!await _playerService.IsEnoughCrypto(playerId))
            return CallResult<bool>.Failure("Not enough USDt");

        await _playerService.BuyPrem(playerId);

        return CallResult<bool>.Success(true);
    }
}