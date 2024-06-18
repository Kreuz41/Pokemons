namespace Pokemons.Core.Providers.TimeProvider;

public class TimeProvider : ITimeProvider
{
    public DateTime Now() => DateTime.UtcNow;

    public long GetSecondsBetweenDateAndNow(DateTime date) =>
        (long)(Now() - date).TotalSeconds;
}