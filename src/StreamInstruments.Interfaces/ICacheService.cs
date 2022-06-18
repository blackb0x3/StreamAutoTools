namespace StreamInstruments.Interfaces;

public interface ICacheService
{
    Task<T> ReadValueAsync<T>(string key, CancellationToken cancellationToken) where T : class;

    Task WriteValueAsync<T>(string key, T value, TimeSpan expiry, bool overwrite, CancellationToken cancellationToken) where T : class;

    Task RemoveValueAsync(string key, CancellationToken cancellationToken);

    Task ClearAsync(CancellationToken cancellationToken);
}