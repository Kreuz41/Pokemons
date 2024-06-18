namespace Pokemons.DataLayer.Cache.Repository;

public interface ICacheRepository
{
    Task SetMember<T>(string key, T data);
    Task<T?> GetMember<T>(string key) where T : class;
    Task<T?> DeleteMember<T>(string key) where T : class;
}