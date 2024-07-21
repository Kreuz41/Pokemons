using System.Collections.Concurrent;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.NotificationRepository;

namespace Pokemons.Core.BackgroundServices.NotificationCreator;

public class NotificationCreator : BackgroundService
{
    public NotificationCreator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    private static readonly ConcurrentBag<Notification> Notifications = [];
    private readonly IServiceScopeFactory _scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            List<Notification> newList;
            
            lock (Notifications)
            {
                newList = [..Notifications];
                Notifications.Clear();
            }
            
            if (newList.Count > 0)
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetService<INotificationRepository>()!;
                await service.AddRangeNotifications(newList);

                foreach (var notification in newList)
                    RabbitMqNotificationSender.RabbitMqNotificationSender.EnqueueNotification(notification);
            }

            await Task.Delay(500, stoppingToken);
        }
    }

    public static void AddNotification(Notification notification) =>
        Notifications.Add(notification);
}