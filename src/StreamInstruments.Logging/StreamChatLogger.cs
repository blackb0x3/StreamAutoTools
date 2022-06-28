using StreamInstruments.Models;

namespace StreamInstruments.Logging;

public class StreamChatLogger : IStreamChatLogger
{
    public void LogChatMessage(string logMessage, string senderUsername, StreamingService streamingService)
    {
        throw new NotImplementedException();
    }

    public void RemoveLoggedMessagesByUser(string senderUsername, StreamingService streamingService)
    {
        throw new NotImplementedException();
    }

    public void RemoveAllLoggedMessages()
    {
        throw new NotImplementedException();
    }
}