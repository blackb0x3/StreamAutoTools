using MediatR;
using StreamInstruments.Logging;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Events;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using OnLogArgs = TwitchLib.Client.Events.OnLogArgs;

namespace StreamInstruments.Hubs.Twitch;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;
    private readonly TwitchPubSub _twitchPubSub;
    private readonly TwitchClient _twitchClient;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IMediator mediator, TwitchPubSub twitchPubSub, TwitchClient twitchClient)
    {
        _logger = logger;
        _configuration = configuration;
        _mediator = mediator;
        _twitchPubSub = twitchPubSub;

        // Initialize() should already have been called at this point
        _twitchClient = twitchClient;
        _twitchClient.OnLog += OnLog!;
        _twitchClient.OnConnected += OnClientConnected!;
        _twitchClient.OnError += OnClientError!;
        _twitchClient.OnJoinedChannel += OnJoinedChannel!;
        _twitchClient.OnMessageReceived += OnMessageReceived!;
        _twitchClient.OnRaidNotification += OnIncomingRaid!;

        // pubsub generic setup
        _twitchPubSub.OnListenResponse += OnPubSubListenResponse!;
        _twitchPubSub.OnPubSubServiceConnected += OnPubSubServiceConnected!;
        _twitchPubSub.OnPubSubServiceClosed += OnPubSubServiceClosed!;
        _twitchPubSub.OnPubSubServiceError += OnPubSubServiceError!;

        // stream events setup
        _twitchPubSub.OnFollow += OnNewFollower!;
        _twitchPubSub.OnBitsReceivedV2 += OnBitDonation!;
        _twitchPubSub.OnChannelSubscription += OnSubscription!;
        _twitchPubSub.OnRaidUpdateV2 += OnOutgoingRaid!;
        _twitchPubSub.OnChannelPointsRewardRedeemed += OnRewardRedeemed!;

        // TODO inject streamer / channel [Id | Username]
        /*_twitchPubSub.ListenToRaid();
        _twitchPubSub.ListenToChannelPoints();
        _twitchPubSub.ListenToSubscriptions();
        _twitchPubSub.ListenToFollows();
        _twitchPubSub.ListenToBitsEventsV2();
        _twitchPubSub.ListenToVideoPlayback();*/
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _twitchClient.Connect();
        _twitchPubSub.Connect();

        while (!stoppingToken.IsCancellationRequested)
        {
        }

        return Task.CompletedTask;
    }

    private void OnLog(object sender, OnLogArgs e)
    {
        _logger.LogDebug(new { msg = "Log event", e.Data });
    }

    private void OnClientConnected(object sender, OnConnectedArgs e)
    {
        _logger.LogDebug( new { msg = "Connected to channel", e } );
    }

    private void OnClientError(object sender, OnErrorEventArgs e)
    {
        _logger.LogError(new
        {
            msg = "Encountered error with Client service",
            errMessage = e.Exception.Message,
            stackTrace = e.Exception.StackTrace
        });
    }

    private void OnPubSubServiceConnected(object sender, EventArgs e)
    {
        _logger.LogDebug( new { msg = "Connected to pubsub" } );
    }

    private void OnPubSubListenResponse(object sender, OnListenResponseArgs e)
    {
        _logger.LogDebug( new { msg = "Topic listen response", e.Topic, e.Successful } );

        if (!e.Successful)
        {
            _logger.LogError( new { msg = "Failed to listen", response = e.Response } );
        }
    }

    private void OnPubSubServiceError(object sender, OnPubSubServiceErrorArgs e)
    {
        _logger.LogError(new
        {
            msg = "Encountered error with PubSub service",
            errMessage = e.Exception.Message,
            stackTrace = e.Exception.StackTrace
        });
    }

    private void OnPubSubServiceClosed(object sender, EventArgs e)
    {
        _logger.LogDebug( new { msg = "Connection closed to pubsub server" } );
    }

    private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        _logger.LogInfo( new { msg = "Joined channel", channelJoined = e.Channel, joinedAs = e.BotUsername } );
    }

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        _logger.LogDebug(new
        {
            msg = "Chat message received"
        });

        // TODO determine if message is command
        // if so, call ExecuteCommandHandler in domain via _mediator and publish result to chat
        // else do nothing
    }

    private void OnNewFollower(object sender, OnFollowArgs e)
    {
        _logger.LogDebug( new { msg = "New follower received", e } );
    }

    private void OnBitDonation(object sender, OnBitsReceivedV2Args e)
    {
        _logger.LogDebug(new
        {
            msg = "Bits donated",
            donatedBy = e.UserName,
            donatedById = e.UserId,
            e.ChannelId,
            e.BitsUsed
        });
    }

    private void OnSubscription(object sender, OnChannelSubscriptionArgs e)
    {
        _logger.LogDebug(new
        {
            msg = "Viewer subscribed",
            e.Subscription.ChannelId,
            e.Subscription.ChannelName,
            e.Subscription.UserId,
            e.Subscription.Username,
            e.Subscription.Months,
            e.Subscription.SubscriptionPlanName,
            e.Subscription.IsGift
        });
    }

    private void OnIncomingRaid(object sender, OnRaidNotificationArgs e)
    {
        var viewerCount = Convert.ToInt32(e.RaidNotification.MsgParamViewerCount);

        _logger.LogDebug(new
        {
            msg = "Incoming raid",
            e.RaidNotification.Id,
            e.RaidNotification.UserId,
            ViewerCount = viewerCount
        });
    }

    /// <remarks>
    /// This will get called whenever the view count updates for an outgoing raid.
    /// </remarks>
    private void OnOutgoingRaid(object sender, OnRaidUpdateV2Args e)
    {
        _logger.LogDebug(new
        {
            msg = "Outgoing raid update",
            e.ChannelId,
            e.TargetChannelId,
            e.ViewerCount
        });
    }

    private void OnRewardRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
    {
        var status = e.RewardRedeemed.Redemption.Status;
        var isUnfulfilled = status.Equals("UNFULFILLED");
        var redemption = e.RewardRedeemed.Redemption;
        var reward = redemption.Reward;

        _logger.LogDebug(new
        {
            msg = isUnfulfilled ? "New reward redemption" : $"Redemption marked as {status}",
            e.ChannelId,
            redemption = reward.Title,
            redeemedFor = reward.Cost,
            redeemedBy = redemption.User.Login
        });
    }
}