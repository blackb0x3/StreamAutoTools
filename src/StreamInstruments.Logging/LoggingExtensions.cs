using Microsoft.Extensions.Logging;

namespace StreamInstruments.Logging;

public static class LoggingExtensions
{
    public static void LogDebug<T>(this ILogger<T> logger, object objToLog)
    {
        logger.LogDebug("{data}", objToLog);
    }

    public static void LogInfo<T>(this ILogger<T> logger, object objToLog)
    {
        logger.LogInformation("{data}", objToLog);
    }

    public static void LogWarning<T>(this ILogger<T> logger, object objToLog)
    {
        logger.LogWarning("{data}", objToLog);
    }

    public static void LogError<T>(this ILogger<T> logger, object objToLog)
    {
        logger.LogError("{data}", objToLog);
    }
}