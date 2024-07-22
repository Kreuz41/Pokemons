using Pokemons.API.CallResult;
using Pokemons.DataLayer.Database.Models.Entities;

namespace Pokemons.API.Handlers;

public interface INotificationHandler
{
    Task<CallResult<IEnumerable<Notification>>> GetNotifications(long playerId, int offset);
    Task<CallResult<bool>> ReadNotification(long playerId, long notificationId);
    Task<CallResult<bool>> ReadAllNotifications(long playerId);
    Task<CallResult<bool>> DeleteAllNotifications(long playerId);
    Task<CallResult<IEnumerable<News>>> GetNews(long playerId, int offset);
    Task<CallResult<bool>> ReadNews(long playerId, long newsId);
    Task<CallResult<bool>> ReadAllNews(long playerId);
    Task<CallResult<bool>> DeleteNotification(long playerId, long notificationId);
}