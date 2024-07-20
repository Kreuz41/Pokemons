using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.DataLayer.MasterRepositories.NotificationRepository;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetNotificationsWithOffset(long playerId, int offset);
    Task<Notification?> GetNotification(long playerId, long notificationId);
    Task UpdateNotification(Notification notification);
    Task<IEnumerable<Notification>> GetAllNotifications(long playerId);
    Task UpdateRangeNotifications(IEnumerable<Notification> notifications);
    Task DeleteAllNotifications(long playerId);
    Task<IEnumerable<News>> GetNews(long playerId, int offset);
    Task<News?> GetNewsById(long playerId, long newsId);
    Task UpdateNews(News news);
    Task<IEnumerable<News>> GetAllNews(long playerId);
    Task UpdateRangeNews(IEnumerable<News> news);
}