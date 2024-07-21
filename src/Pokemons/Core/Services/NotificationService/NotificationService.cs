using Pokemons.Core.BackgroundServices.RabbitMqNotificationSender;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.MasterRepositories.NotificationRepository;
using PokemonsDomain.Notification;

namespace Pokemons.Core.Services.NotificationService;

public class NotificationService : INotificationService
{
    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    private static INotificationRepository _notificationRepository = null!;

    public async Task<IEnumerable<Notification>> GetNotifications(long playerId, int offset) =>
        await _notificationRepository.GetNotificationsWithOffset(playerId, offset);

    public async Task<bool> IsNotificationExist(long playerId, long notificationId) =>
        await _notificationRepository.GetNotification(playerId, notificationId) != null;

    public async Task ReadNotification(long playerId, long notificationId)
    {
        var notification = await _notificationRepository.GetNotification(playerId, notificationId);
        if (notification is null) return;

        notification.IsRead = true;
        await _notificationRepository.UpdateNotification(notification);
    }

    public async Task ReadAllNotifications(long playerId)
    {
        var notifications = await _notificationRepository.GetAllNotifications(playerId);
        var enumerable = notifications as Notification[] ?? notifications.ToArray();
        foreach (var notification in enumerable)
        {
            notification.IsRead = true;
        }

        await _notificationRepository.UpdateRangeNotifications(enumerable);
    }

    public async Task DeleteAllNotifications(long playerId) =>
        await _notificationRepository.DeleteAllNotifications(playerId);

    public async Task<IEnumerable<News>> GetNews(long playerId, int offset) =>
        await _notificationRepository.GetNews(playerId, offset);

    public async Task<bool> IsNewsExist(long playerId, long newsId)
    {
        throw new NotImplementedException();
    }

    public async Task ReadNews(long playerId, long newsId)
    {
        var news = await _notificationRepository.GetNewsById(playerId, newsId);
        if (news is null) return;

        news.IsRead = true;
        await _notificationRepository.UpdateNews(news);
    }

    public async Task ReadAllNews(long playerId)
    {
        var news = await _notificationRepository.GetAllNews(playerId);

        var newsEnumerable = news as News[] ?? news.ToArray();
        foreach (var value in newsEnumerable)
        {
            value.IsRead = true;
        }

        await _notificationRepository.UpdateRangeNews(newsEnumerable);
    }
}