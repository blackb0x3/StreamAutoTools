using MediatR;
using StreamInstruments.DataObjects;
using StreamInstruments.Helpers;
using StreamInstruments.Hubs.Commands.Infrastructure.Helpers;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;
using StreamInstruments.Interfaces;
using StreamInstruments.Models;

namespace StreamInstruments.Hubs.Commands.Infrastructure.Adapters;

internal class GetCommandAvailabilityQueryHandler : IRequestHandler<GetCommandAvailabilityQuery, GetCommandAvailabilityResponse>
{
    private readonly ICacheService _cacheService;

    public GetCommandAvailabilityQueryHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<GetCommandAvailabilityResponse> Handle(GetCommandAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var commandAvailability = await GetCommandAvailabilityAsync(request.Command, request.SenderUsername,
            request.StreamingService, cancellationToken);

        return new GetCommandAvailabilityResponse { CommandAvailability = commandAvailability };
    }

    private async Task<CommandAvailability> GetCommandAvailabilityAsync(Command command, string senderUsername, StreamingService streamingService, CancellationToken cancellationToken)
    {
        if (!command.IsActive || command.IsDeleted)
        {
            return CommandAvailability.NotAvailable;
        }

        var streamServiceCacheKeyPart = StreamingServiceHelper.GetCacheKeyPartFromStreamingService(streamingService);
        var userInfoKey = string.Join('-', CacheKeyConstants.UserInfoKeyPart, streamServiceCacheKeyPart, senderUsername);
        var userInfo = await _cacheService.ReadValueAsync<object>(userInfoKey, cancellationToken);

        // until we add a Twitch API client, assume it's being used by anyone for now
        var userLevel = ViewerLevel.Everyone;

        // broadcaster should not be affected by access levels and / or cooldowns
        // so exit early if this is the case
        if (userLevel != ViewerLevel.Broadcaster)
        {
            if (userLevel < command.AccessLevel)
            {
                return CommandAvailability.AccessRestricted;
            }

            var globalCooldownCacheKey = CooldownKeyHelper.ConstructCooldownKey(command.Id, CacheKeyConstants.GlobalCooldownIdentifier);
            var isOnGlobalCooldown = await _cacheService.ReadValueAsync<bool>(globalCooldownCacheKey, cancellationToken);

            if (isOnGlobalCooldown)
            {
                return CommandAvailability.OnGlobalCooldown;
            }

            var viewerCooldownCacheKey = CooldownKeyHelper.ConstructCooldownKey(command.Id, senderUsername);
            var isOnViewerCooldown = await _cacheService.ReadValueAsync<bool>(globalCooldownCacheKey, cancellationToken);

            if (isOnViewerCooldown)
            {
                return CommandAvailability.OnViewerCooldown;
            }
        }

        return CommandAvailability.Available;
    }
}