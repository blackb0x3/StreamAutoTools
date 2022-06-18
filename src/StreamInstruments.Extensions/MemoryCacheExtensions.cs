using System.Reflection;
using Microsoft.Extensions.Caching.Memory;

namespace StreamInstruments.Extensions;

public static class MemoryCacheExtensions
{
    private const string ClearMethodName = "Clear";
    private const string EntriesCollectionPropertyName = "EntriesCollection";

    /// <summary>
    /// Clears the IMemoryCache of all entries, effectively: emptying everything stored within it.
    /// </summary>
    /// <param name="cache">Cache</param>
    /// <exception cref="InvalidOperationException">Unable to clear memory cache</exception>
    /// <exception cref="ArgumentNullException">Cache is null</exception>
    public static void Clear(this IMemoryCache cache)
    {
        if (cache is MemoryCache memCache)
        {
            memCache.Compact(1.0);
            return;
        }

        var cacheType = cache.GetType();

        MethodInfo? clearMethod = cacheType.GetMethod(ClearMethodName, BindingFlags.Instance | BindingFlags.Public);

        if (clearMethod is not null)
        {
            clearMethod.Invoke(cache, null);
            return;
        }

        PropertyInfo? prop = cacheType.GetProperty(EntriesCollectionPropertyName, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public);

        if (prop != null)
        {
            object? innerCache = prop.GetValue(cache);

            if (innerCache != null)
            {
                clearMethod = innerCache.GetType().GetMethod(ClearMethodName, BindingFlags.Instance | BindingFlags.Public);

                if (clearMethod != null)
                {
                    clearMethod.Invoke(innerCache, null);
                    return;
                }
            }
        }

        throw new InvalidOperationException($"Unable to clear memory cache instance of type {cacheType.FullName}");
    }
}