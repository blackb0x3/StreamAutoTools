namespace StreamInstruments.DataObjects;

public enum ActionType
{
    /// <summary>
    /// Turns on follower-only chat on Twitch.
    /// </summary>
    TurnOnFollowersOnlyChat,
    /// <summary>
    /// Turns off follower-only chat on Twitch.
    /// </summary>
    TurnOffFollowersOnlyChat,
    /// <summary>
    /// Turns on subscriber-only chat on Twitch.
    /// </summary>
    TurnOnSubscribersOnlyChat,
    /// <summary>
    /// Turns off subscriber-only chat on Twitch.
    /// </summary>
    TurnOffSubscribersOnlyChat,
    /// <summary>
    /// Turns on emote-only chat on Twitch.
    /// </summary>
    TurnOnEmoteOnlyChat,
    /// <summary>
    /// Turns off emote-only chat on Twitch.
    /// </summary>
    TurnOffEmoteOnlyChat,
    /// <summary>
    /// Toggles slow mode on Twitch. On or off depending on it's current state in the Twitch chat.
    /// </summary>
    ToggleSlowMode,
    /// <summary>
    /// Sends a message to Twitch chat.
    /// </summary>
    SendMessage,
    /// <summary>
    /// Disables a viewer's ability to post in Twitch chat for a given interval in seconds.
    /// </summary>
    TimeoutViewer,
    /// <summary>
    /// Permanently disables a viewer's ability to interact with a Twitch stream.
    /// </summary>
    BanViewer
}