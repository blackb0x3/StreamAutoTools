using StreamInstruments.Models;

namespace StreamInstruments.Logging;

public interface IStreamChatLogger
{
    void LogChatMessage(string logMessage, string senderUsername, StreamingService streamingService);

    void RemoveLoggedMessagesByUser(string senderUsername, StreamingService streamingService);

    void RemoveAllLoggedMessages();
}