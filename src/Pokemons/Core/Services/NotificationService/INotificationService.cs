using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.Services.NotificationService;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetNotifications(long playerId, int offset);
    Task<bool> IsNotificationExist(long playerId, long notificationId);
    Task ReadNotification(long playerId, long notificationId);
    Task ReadAllNotifications(long playerId);
    Task DeleteAllNotifications(long playerId);
    Task<IEnumerable<News>> GetNews(long playerId, int offset);
    Task<bool> IsNewsExist(long playerId, long newsId);
    Task ReadNews(long playerId, long newsId);
    Task ReadAllNews(long playerId);
}