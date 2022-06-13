namespace StreamInstruments.DataObjects;

public class Reward : EntityBase
{
    /// <summary>
    /// The Twitch ID of this channel reward for <see cref="Streamer"/>.
    /// </summary>
    public string TwitchRewardId { get; set; }

    /// <summary>
    /// The title of the channel reward.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Determines whether auto completions for this reward should be paused.
    /// </summary>
    public bool PauseAutoCompletions { get; set; }

    /// <summary>
    /// Sets the interval for how often to autocomplete redemptions (in seconds) for this reward
    /// currently in the queue.
    /// </summary>
    public int AutoCompletionInterval { get; set; }
}