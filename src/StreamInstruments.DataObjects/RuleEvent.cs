namespace StreamInstruments.DataObjects;

public enum RuleEvent
{
    /// <summary>
    /// Activates when a streamer goes live.
    /// </summary>
    StreamStart,
    /// <summary>
    /// Activates when a streamer goes offline.
    /// </summary>
    StreamEnd,
    /// <summary>
    /// Activates when a new viewer follows a channel.
    /// </summary>
    NewFollower,
    /// <summary>
    /// Activates when a viewer donates Twitch bits as part of a chat message.
    /// </summary>
    BitDonation,
    /// <summary>
    /// Activates when a viewer subscribes to a channel.
    /// </summary>
    Subscription,
    /// <summary>
    /// Activates when a viewer gifts one or more subs to a channel.
    /// </summary>
    GiftedSub,
    /// <summary>
    /// Activates when a streamer receives a raid from another streamer on Twitch.
    /// </summary>
    IncomingRaid,
    /// <summary>
    /// Activates when a streamer is about to perform a raid to another streamer's channel on Twitch.
    /// </summary>
    OutgoingRaid,
    /// <summary>
    /// Activates when a streamer's Twitch channel reward is redeemed by a viewer.
    /// </summary>
    RewardRedeemed,
}