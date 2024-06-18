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
    
    public async Task SetMember<T>(string key, T data) =>
        await _database.StringSetAsync(GetKey<T>(key), 
            JsonSerializer.Serialize(data));

    public async Task<T?> GetMember<T>(string key) where T : class
    {
        var member = await _database.StringGetAsync(GetKey<T>(key));
        if (member.IsNullOrEmpty || !member.HasValue) return null;
        
        return JsonSerializer.Deserialize<T>(member!);
    }

    public async Task<T?> DeleteMember<T>(string key) where T : class
    {
        var member = await _database.StringGetDeleteAsync(GetKey<T>(key));
        if (member.IsNullOrEmpty || !member.HasValue) return null;
        
        return JsonSerializer.Deserialize<T>(member!);
    }
    
    private static string GetKey<T>(string key) => typeof(T) + ":" + key;
}