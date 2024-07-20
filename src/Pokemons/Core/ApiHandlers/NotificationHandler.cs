using Pokemons.API.CallResult;
using Pokemons.API.Handlers;
using Pokemons.Core.Services.NotificationService;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.Core.ApiHandlers;

public class NotificationHandler : INotificationHandler
{
    public NotificationHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    private readonly INotificationService _notificationService;
    
    public async Task<CallResult<IEnumerable<Notification>>> GetNotifications(long playerId, int offset)
    {
        if (offset < 0)
            return CallResult<IEnumerable<Notification>>.Failure("Invalid offset");
        
        var notifications = await _notificationService.GetNotifications(playerId, offset);
        return CallResult<IEnumerable<Notification>>.Success(notifications);
    }

    public async Task<CallResult<bool>> ReadNotification(long playerId, long notificationId)
    {
        if (!await _notificationService.IsNotificationExist(playerId, notificationId))
            return CallResult<bool>.Failure("Notification not found");

        await _notificationService.ReadNotification(playerId, notificationId);

        return CallResult<bool>.Success(true);
    }

    public async Task<CallResult<bool>> ReadAllNotifications(long playerId)
    {
        await _notificationService.ReadAllNotifications(playerId);
        return CallResult<bool>.Success(true);
    }

    public async Task<CallResult<bool>> DeleteAllNotifications(long playerId)
    {
        await _notificationService.DeleteAllNotifications(playerId);
        return CallResult<bool>.Success(true);
    }

    public async Task<CallResult<IEnumerable<News>>> GetNews(long playerId, int offset)
    {
        if (offset < 0)
            return CallResult<IEnumerable<News>>.Failure("Invalid offset");
        
        var news = await _notificationService.GetNews(playerId, offset);
        return CallResult<IEnumerable<News>>.Success(news);
    }

    public async Task<CallResult<bool>> ReadNews(long playerId, long newsId)
    {
        if (!await _notificationService.IsNewsExist(playerId, newsId))
            return CallResult<bool>.Failure("News not found");

        await _notificationService.ReadNews(playerId, newsId);
        
        return CallResult<bool>.Success(true);
    }

    public async Task<CallResult<bool>> ReadAllNews(long playerId)
    {
        await _notificationService.ReadAllNews(playerId);
        return CallResult<bool>.Success(true);
    }
}