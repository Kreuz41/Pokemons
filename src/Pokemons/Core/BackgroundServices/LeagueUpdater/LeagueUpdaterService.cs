using Pokemons.Core.Providers.TimeProvider;
using Pokemons.Core.Services.RatingService;

namespace Pokemons.Core.BackgroundServices.LeagueUpdater;

public class LeagueUpdaterService : BackgroundService
{
    public LeagueUpdaterService(IServiceScopeFactory scopeFactory, ITimeProvider timeProvider)
    {
        _scopeFactory = scopeFactory;
        _timeProvider = timeProvider;
    }
    
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ITimeProvider _timeProvider;

    public static bool IsLeagueUpdating { get; private set; } = false;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer  = new PeriodicTimer(_timeProvider.GetTimeSpanForLeagueUpdater());
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            IsLeagueUpdating = true;
            using var scope = _scopeFactory.CreateScope();
            var ratingService = scope.ServiceProvider.GetService<IRatingService>();
            await ratingService?.StartUpdateLeagueRating()!;
            IsLeagueUpdating = false;
        }
    }
}