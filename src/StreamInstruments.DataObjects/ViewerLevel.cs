namespace StreamInstruments.DataObjects;

public enum ViewerLevel
{
    /// <summary>
    /// Anyone can use the command.
    /// </summary>
    Everyone,
    /// <summary>
    /// Anyone who follows the streamer's channel can use the command.
    /// </summary>
    Followers,
    /// <summary>
    /// Anyone who is subscribed to the streamer's channel can use the command.
    /// </summary>
    Subscribers,
    /// <summary>
    /// Any VIPs of the streamer's channel can use the command.
    /// </summary>
    Vips,
    /// <summary>
    /// Any moderators of the streamer's channel can use the command.
    /// </summary>
    Moderators,
    /// <summary>
    /// Only the streamer can use the command.
    /// </summary>
    Broadcaster
}