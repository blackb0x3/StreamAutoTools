using Microsoft.Extensions.Caching.Memory;
using OneOf;
using StreamInstruments.Extensions;
using StreamInstruments.Interfaces;

namespace StreamInstruments.Services.Caching;

public class InMemoryCacheService : ICacheService
{
    private readonly IMemoryCache _innerCache;

    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public InMemoryCacheService(IMemoryCache innerCache)
    {
        _innerCache = innerCache;
    }

    public async Task<T> ReadValueAsync<T>(string key, CancellationToken cancellationToken)
    {
        return await PerformCacheOperationAsync(() =>
        {
            var entry = _innerCache.Get<T>(key);

            return entry;
        }, cancellationToken);
    }

    public async Task WriteValueAsync<T>(string key, T value, TimeSpan expiry, bool overwrite, CancellationToken cancellationToken)
    {
        await PerformCacheOperationAsync(() =>
        {
            var entry = _innerCache.Get<T>(key);

            if (entry is null || overwrite)
            {
                _innerCache.Set(key, value, expiry);
            }

            return entry;
        }, cancellationToken);
    }

    public async Task RemoveValueAsync(string key, CancellationToken cancellationToken)
    {
        await PerformCacheOperationAsync(() =>
        {
            var exists = _innerCache.TryGetValue(key, out var entry);

            if (exists)
            {
                _innerCache.Remove(key);
            }

            return entry;
        }, cancellationToken);
    }

    public async Task ClearAsync(CancellationToken cancellationToken)
    {
        await PerformCacheOperationAsync(() =>
        {
            _innerCache.Clear();

            return 0;
        }, cancellationToken);
    }

    private static async Task<TResult> PerformCacheOperationAsync<TResult>(Func<TResult> func, CancellationToken cancellationToken)
    {
        try
        {
            await Semaphore.WaitAsync(cancellationToken);

            return func();
        }
        finally
        {
            Semaphore.Release();
        }
    }
}