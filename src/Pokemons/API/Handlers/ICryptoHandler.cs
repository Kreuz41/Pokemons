namespace Pokemons.API.Handlers;

public interface ICryptoHandler
{
    Task<CallResult.CallResult<bool>> BuyPrem(long playerId);
}