using System.ComponentModel;
using System.Text.Json;
using Pokemons.Core.Providers.TimeProvider;
using Pokemons.DataLayer.Database.Models.Entities;
using StackExchange.Redis;

namespace Pokemons.DataLayer.Cache.Repository;

public class CacheRepository : ICacheRepository
{
    public CacheRepository(IConnectionMultiplexer redis, ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _database = redis.GetDatabase();
    }

    private readonly IDatabase _database;
    private readonly ITimeProvider _timeProvider;
    
    public async Task SetMember<T>(string key, T data, int? minutes = null, Func<string>? keyPattern = null)
    {
        await _database.StringSetAsync(GetKey<T>(key) + keyPattern?.Invoke(),
            JsonSerializer.Serialize(data),
            minutes is null ? null : _timeProvider.GetTimeForCacheLifeTime(minutes.Value));
    }

    public async Task<T?> GetMember<T>(string key, Func<string>? keyPattern = null) where T : class
    {
        var member = await _database.StringGetAsync(GetKey<T>(key) + keyPattern?.Invoke());
        if (member.IsNullOrEmpty || !member.HasValue) return null;
        
        return JsonSerializer.Deserialize<T>(member!);
    }

    public async Task<T?> DeleteMember<T>(string key, Func<string>? keyPattern = null) where T : class
    {
        var member = await _database.StringGetDeleteAsync(GetKey<T>(key) + keyPattern?.Invoke());
        if (member.IsNullOrEmpty || !member.HasValue) return null;
        
        return JsonSerializer.Deserialize<T>(member!);
    }
    
    private static string GetKey<T>(string key) => $"{typeof(T)}:{key}.";
}