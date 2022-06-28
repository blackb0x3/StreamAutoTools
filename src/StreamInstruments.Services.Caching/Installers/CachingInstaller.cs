using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using StreamInstruments.Interfaces;

namespace StreamInstruments.Services.Caching.Installers;

public static class CachingInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddSingleton<IMemoryCache, MemoryCache>();
        services.AddSingleton<ICacheService, InMemoryCacheService>();
    }
}