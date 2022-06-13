namespace StreamInstruments.DataObjects;

public enum ResponseDestination
{
    /// <summary>
    /// Sends the response to Twitch Chat.
    /// </summary>
    TwitchChatWindow,
    /// <summary>
    /// Sends the message privately to whoever used the command.
    /// </summary>
    TwitchWhisper,
}