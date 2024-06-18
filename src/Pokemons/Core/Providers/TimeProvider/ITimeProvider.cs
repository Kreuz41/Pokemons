namespace Pokemons.Core.Providers.TimeProvider;

public interface ITimeProvider
{
    DateTime Now();
    long GetSecondsBetweenDateAndNow(DateTime date);
}