using Microsoft.EntityFrameworkCore;
using Pokemons.DataLayer.Cache.Repository;
using Pokemons.DataLayer.Database;
using Pokemons.DataLayer.Database.Models.Entities;
using Pokemons.DataLayer.Database.Repositories.UnitOfWork;

namespace Pokemons.DataLayer.MasterRepositories.NotificationRepository;

public class NotificationRepository : INotificationRepository
{
    private readonly ICacheRepository _cacheRepository;
    private const int CacheLifeTime = 5;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationRepository(ICacheRepository cacheRepository, AppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _cacheRepository = cacheRepository;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Notification>> GetNotificationsWithOffset(long playerId, int offset)
    {
        var notifications = await GetAllNotifications(playerId);

        return notifications.ToList().Take(new Range(offset * 10, 10));
    }

    public async Task<Notification?> GetNotification(long playerId, long notificationId)
    {
        var notifies = await GetAllNotifications(playerId);

        return notifies.FirstOrDefault(n => n.Id == notificationId && n.PlayerId == playerId);
    }

    public async Task UpdateNotification(Notification notification)
    {
        if (await GetAllNotifications(notification.PlayerId) is List<Notification> notifies)
        {
            var index = notifies.FindIndex(n => n.Id == notification.Id);
            if (index >= 0)
                notifies[index] = notification;
            
            await _cacheRepository.SetMember(notification.PlayerId.ToString(), 
                notifies as IEnumerable<Notification>);
        }
    }

    public async Task<IEnumerable<Notification>> GetAllNotifications(long playerId)
    {
        var notifications = await _cacheRepository.GetMember<IEnumerable<Notification>>(playerId.ToString());
        if (notifications is not null) return notifications;

        notifications = await _context.Notifications
            .Where(n => n.PlayerId == playerId)
            .ToListAsync();

        await _cacheRepository.SetMember(playerId.ToString(), notifications, CacheLifeTime);
        return notifications;
    }

    public async Task UpdateRangeNotifications(IEnumerable<Notification> notifications)
    {
        var enumerable = notifications.ToList();
        if (enumerable.Count == 0) return;
        var playerId = enumerable.FirstOrDefault()!.PlayerId;

        await _cacheRepository.SetMember(playerId.ToString(), enumerable, CacheLifeTime);

        await _unitOfWork.BeginTransaction();
        _context.Notifications.UpdateRange(enumerable);
        await _unitOfWork.CommitTransaction();
    }

    public async Task DeleteAllNotifications(long playerId)
    {
        await _cacheRepository.DeleteMember<IEnumerable<Notification>>(playerId.ToString());

        await _unitOfWork.BeginTransaction();
        await _context.Notifications
            .Where(n => n.PlayerId == playerId)
            .ExecuteDeleteAsync();
        await _unitOfWork.CommitTransaction();
    }

    public async Task<IEnumerable<News>> GetNews(long playerId, int offset)
    {
        var news = await GetAllNews(playerId);
        
        return news.ToList().Take(new Range(offset * 10, 10));
    }

    public async Task<News?> GetNewsById(long playerId, long newsId)
    {
        var news = await GetAllNews(playerId);
        return news.FirstOrDefault(n => n.Id == newsId && n.PlayerId == playerId);
    }

    public async Task UpdateNews(News news)
    {
        if (await GetAllNews(news.PlayerId) is List<News> newsList)
        {
            var index = newsList.FindIndex(n => n.Id == news.Id);
            if (index > 0)
                newsList[index] = news;
            
            await _cacheRepository.SetMember(news.PlayerId.ToString(), 
                newsList as IEnumerable<News>);
        }
    }

    public async Task<IEnumerable<News>> GetAllNews(long playerId)
    {
        var news = await _cacheRepository.GetMember<IEnumerable<News>>(playerId.ToString());
        if (news is not null) return news;

        news = await _context.News.Where(n => n.PlayerId == playerId).ToListAsync();

        await _cacheRepository.SetMember(playerId.ToString(), news, CacheLifeTime);
        return news;
    }

    public async Task UpdateRangeNews(IEnumerable<News> news)
    {
        var enumerable = news.ToList();
        if (enumerable.Count == 0) return;
        var playerId = enumerable.FirstOrDefault()!.PlayerId;

        await _cacheRepository.SetMember(playerId.ToString(), enumerable, CacheLifeTime);

        await _unitOfWork.BeginTransaction();
        _context.News.UpdateRange(enumerable);
        await _unitOfWork.CommitTransaction();
    }

    public async Task CreateNotification(Notification notification)
    {
        await _unitOfWork.BeginTransaction();
        await _context.Notifications.AddAsync(notification);
        await _unitOfWork.CommitTransaction();
    }

    public async Task AddRangeNotifications(List<Notification> newList)
    {
        await _unitOfWork.BeginTransaction();
        _context.Notifications.UpdateRange(newList);
        await _unitOfWork.CommitTransaction();
    }
}