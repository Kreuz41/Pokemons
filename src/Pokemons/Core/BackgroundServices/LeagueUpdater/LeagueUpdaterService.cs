namespace Pokemons.Core.BackgroundServices.LeagueUpdater;

public class LeagueUpdaterService : BackgroundService
{
    public LeagueUpdaterService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    private readonly IServiceScopeFactory _scopeFactory;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromHours(6));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            
        }
    }
}