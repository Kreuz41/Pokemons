using System.ComponentModel;
using System.Text.Json;
using Pokemons.DataLayer.Database.Models.Entities;
using StackExchange.Redis;

namespace Pokemons.DataLayer.Cache.Repository;

public class CacheRepository : ICacheRepository
{
    public CacheRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
        _subscriber ??= redis.GetSubscriber();
    }

    private readonly IDatabase _database;
    private static ISubscriber? _subscriber;
    
    public async Task SetMember<T>(string key, T data, int? minutes = null, Func<string>? keyPattern = null)
    {
        await _database.StringSetAsync(GetKey<T>(key) + keyPattern?.Invoke(),
            JsonSerializer.Serialize(data),
            minutes is null ? null : TimeSpan.FromMinutes((double)minutes));
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