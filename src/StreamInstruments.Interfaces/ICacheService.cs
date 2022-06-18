using StreamInstruments.Models;

namespace StreamInstruments.Interfaces;

public interface ICacheService
{
    bool KeyExists(string key);

    Task<CacheServiceReadResult<T>> ReadValueAsync<T>(string key) where T : class;

    Task<CacheServiceWriteResult> WriteValueAsync<T>(string key, T value, TimeSpan expiry, bool overwrite) where T : class;

    Task<CacheServiceRemoveResult> RemoveValueAsync(string key);

    Task<CacheServiceRemoveAllResult> RemoveAllValuesAsync();
}