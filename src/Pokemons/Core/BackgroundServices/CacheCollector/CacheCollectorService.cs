using Pokemons.API.Handlers;
using Pokemons.Core.Helpers;
using Pokemons.Core.Providers.TimeProvider;

namespace Pokemons.Core.BackgroundServices.CacheCollector;

public class CacheCollectorService : BackgroundService
{
    public CacheCollectorService(IServiceScopeFactory scopeFactory, ITimeProvider timeProvider)
    {
        _scopeFactory = scopeFactory;
        _timeProvider = timeProvider;
    }

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ITimeProvider _timeProvider;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(_timeProvider.GetTimeSpanForCacheCollecting());

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetService<IAuthHandler>()!;
            
            foreach (var playerId in SessionHelper.GetEnded())
                await service.EndSession(playerId);
        }
    }
}