using StreamInstruments.Models;

namespace StreamInstruments.Helpers;

public class StreamingServiceHelper
{
    public static string GetCacheKeyPartFromStreamingService(StreamingService streamingService)
    {
        return streamingService switch
        {
            StreamingService.Twitch => "twitch",
            StreamingService.YouTube => "youtube",
            _ => throw new ArgumentOutOfRangeException($"Unrecognised streaming service {streamingService.ToString()}")
        };
    }
}