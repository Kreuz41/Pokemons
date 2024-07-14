using System.Collections.Concurrent;
using Pokemons.API.Handlers;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Cache.Repository;

namespace Pokemons.Core.BackgroundServices.CacheCollector;

public class CacheCollectorService : BackgroundService
{
    public CacheCollectorService(IServiceScopeFactory scopeFactory, ITimeProvider timeProvider, ICacheRepository cacheRepository)
    {
        _scopeFactory = scopeFactory;
        _timeProvider = timeProvider;
        _cacheRepository = cacheRepository;
    }

    private readonly IServiceScopeFactory _scopeFactory;
    private static ICacheRepository _cacheRepository = null!;
    private static ITimeProvider _timeProvider = null!;
    private static ConcurrentDictionary<long, DateTime> Sessions { get; set; } = new();
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(_timeProvider.GetTimeSpanForCacheCollecting());

        Sessions = await _cacheRepository.GetMember<ConcurrentDictionary<long, DateTime>>(nameof(Sessions)) 
                   ?? new ConcurrentDictionary<long, DateTime>();

        while (await timer.WaitForNextTickAsync(stoppingToken))
        { 
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetService<IAuthHandler>()!;
            
            foreach (var playerId in await GetEnded())
                await service.EndSession(playerId);
        }
    }

    public static async void UpdateSession(long id)
    {
        Sessions.AddOrUpdate(id, _timeProvider.Now(), (_, _) => _timeProvider.Now());
        await _cacheRepository.SetMember(nameof(Sessions), Sessions);
    }

    private static async Task<IEnumerable<long>> GetEnded()
    {
        var now = _timeProvider.Now();
        var sessionLifetime = _timeProvider.GetSessionLifeTime();
            
        var endedSessions = Sessions
            .Where(p => now - p.Value > sessionLifetime)
            .Select(p => p.Key)
            .ToList();

        foreach (var sessionId in endedSessions)
        {
            Sessions.TryRemove(sessionId, out _);
        }

        await _cacheRepository.SetMember(nameof(Sessions), Sessions);
        
        return endedSessions;
    }
}