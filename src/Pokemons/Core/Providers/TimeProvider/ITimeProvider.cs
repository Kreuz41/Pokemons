namespace Pokemons.Core.Providers.TimeProvider;

public interface ITimeProvider
{
    DateTime Now();
    long GetSecondsBetweenDateAndNow(DateTime date);
    TimeSpan GetTimeSpanForCacheCollecting();
    TimeSpan GetTimeSpanForLeagueUpdater();
    TimeSpan GetTimeForCacheLifeTime(int minutes);
    TimeSpan GetSessionLifeTime();
}