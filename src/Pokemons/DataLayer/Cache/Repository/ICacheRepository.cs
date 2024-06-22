namespace Pokemons.DataLayer.Cache.Repository;

public interface ICacheRepository
{
    Task SetMember<T>(string key, T data, int? minutes = null, Func<string>? keyPattern = null);
    Task<T?> GetMember<T>(string key, Func<string>? keyPattern = null) where T : class;
    Task<T?> DeleteMember<T>(string key, Func<string>? keyPattern = null) where T : class;
}   