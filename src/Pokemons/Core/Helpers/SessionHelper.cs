using System.Collections.Concurrent;
using Pokemons.Core.Providers.TimeProvider;
using TimeProvider = Pokemons.Core.Providers.TimeProvider.TimeProvider;

namespace Pokemons.Core.Helpers;

public static class SessionHelper
{
    private static ConcurrentDictionary<long, DateTime> _sessions { get; set; } = new();
    private static ITimeProvider _timeProvider { get; set; } = new TimeProvider();

    public static void UpdateSession(long id) =>
        _sessions.AddOrUpdate(id, _timeProvider.Now(), (id, time) => _timeProvider.Now());

    public static IEnumerable<long> GetEnded()
    {
        var now = _timeProvider.Now();
        var sessionLifetime = _timeProvider.GetSessionLifeTime();
            
        var endedSessions = _sessions
            .Where(p => now - p.Value > sessionLifetime)
            .Select(p => p.Key)
            .ToList();

        foreach (var sessionId in endedSessions)
        {
            _sessions.TryRemove(sessionId, out _);
        }

        return endedSessions;
    }
}