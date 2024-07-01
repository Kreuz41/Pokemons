using Pokemons.Core.Services.RatingService;

namespace Pokemons.Core.BackgroundServices.LeagueUpdater;

public class LeagueUpdaterService : BackgroundService
{
    public LeagueUpdaterService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    private readonly IServiceScopeFactory _scopeFactory;

    public static bool IsLeagueUpdating { get; private set; } = false;
    private PeriodicTimer Timer { get; set; } = new(TimeSpan.FromMinutes(30));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await Timer.WaitForNextTickAsync(stoppingToken))
        {
            IsLeagueUpdating = true;
            using var scope = _scopeFactory.CreateScope();
            var ratingService = scope.ServiceProvider.GetService<IRatingService>();
            await ratingService?.StartUpdateLeagueRating()!;
            IsLeagueUpdating = false;
        }
    }
}