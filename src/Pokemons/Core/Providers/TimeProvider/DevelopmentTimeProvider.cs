namespace Pokemons.Core.Providers.TimeProvider;

public class DevelopmentTimeProvider: ITimeProvider
{
    public DateTime Now() =>
        DateTime.UtcNow;

    public long GetSecondsBetweenDateAndNow(DateTime date) =>
        (long)(Now() - date).TotalSeconds;

    public TimeSpan GetTimeSpanForCacheCollecting() =>
        TimeSpan.FromSeconds(30);

    public TimeSpan GetTimeSpanForLeagueUpdater() => 
        TimeSpan.FromMinutes(30);

    public TimeSpan GetTimeForCacheLifeTime(int minutes) => 
        TimeSpan.FromMinutes(minutes);

    public TimeSpan GetSessionLifeTime() =>
        TimeSpan.FromSeconds(30);
}