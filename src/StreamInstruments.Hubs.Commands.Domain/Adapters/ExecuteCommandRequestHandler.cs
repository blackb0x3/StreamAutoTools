using MediatR;
using OneOf;
using StreamInstruments.DataObjects;
using StreamInstruments.Extensions;
using StreamInstruments.Helpers;
using StreamInstruments.Hubs.Commands.Domain.PrimaryPorts.ExecuteCommand;
using StreamInstruments.Hubs.Commands.Domain.Representations;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandByName;
using StreamInstruments.Interfaces;
using StreamInstruments.Models;

namespace StreamInstruments.Hubs.Commands.Domain.Adapters;

internal class ExecuteCommandRequestHandler : IRequestHandler<ExecuteCommandRequest, OneOf<ExecuteCommandResponse, ErrorResponse>>
{
    private const string GlobalCooldownIdentifier = "STREAM_INSTRUMENTS_GLOBAL";

    private readonly IMediator _mediator;
    private readonly ICacheService _cacheService;

    public ExecuteCommandRequestHandler(IMediator mediator, ICacheService cacheService)
    {
        _mediator = mediator;
        _cacheService = cacheService;
    }

    public async Task<OneOf<ExecuteCommandResponse, ErrorResponse>> Handle(ExecuteCommandRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await HandleAsync(request, cancellationToken);
        }
        catch (Exception e)
        {
            return new ErrorResponse { Message = e.Message };
        }
    }

    private async Task<OneOf<ExecuteCommandResponse, ErrorResponse>> HandleAsync(
        ExecuteCommandRequest request, 
        CancellationToken cancellationToken
    )
    {
        // Get the command to execute
        var getCommandResp = await GetCommandAsync(request.CommandName, cancellationToken);

        if (getCommandResp.IsT1)
        {
            // early exit if command doesn't exist
            return getCommandResp.AsT1;
        }

        var command = getCommandResp.AsT0;

        // If request is not a dry run
        //    - check for command availability (right access level, no cooldown etc.)
        if (!request.IsDryRun)
        {
            var commandAvailability = await GetCommandAvailabilityAsync(command, request.SenderUsername, request.StreamingService, cancellationToken);

            if (commandAvailability != CommandAvailability.Available)
            {
                return new ErrorResponse
                {
                    // There's no point sending a message if command shouldn't be run. The whole point of
                    // cooldown, access levels etc. is to mitigate bot messages flooding chat for no reason.
                    Message = $"Command {request.CommandName} cannot be used. Reason: {commandAvailability.ToDescription()}"
                };
            }
        }

        // Parse the command text

        // Before returning - ensure we activate the cooldowns
        await ActivateCommandCooldownsAsync(command, request.SenderUsername, cancellationToken);

        // Return a response which includes the parsed command text
        return new ExecuteCommandResponse
        {
            Output = string.Empty
        };
    }

    private async Task<OneOf<Command, ErrorResponse>> GetCommandAsync(string commandName, CancellationToken cancellationToken)
    {
        var getCommandQuery = new GetCommandByNameQuery { CommandName = commandName };

        var command = await _mediator.Send(getCommandQuery, cancellationToken);

        if (command is null)
        {
            return new ErrorResponse { Message = $"Command {commandName} does not exist." };
        }

        return command;
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

        // until we implement a Twitch API, assume it's being used by anyone for now
        var userLevel = ViewerLevel.Everyone;

        // broadcaster should not be affected by access levels and / or cooldowns
        // so exit early if this is the case
        if (userLevel != ViewerLevel.Broadcaster)
        {
            if (userLevel < command.AccessLevel)
            {
                return CommandAvailability.AccessRestricted;
            }

            var isOnGlobalCooldown = await DetermineCommandIsOnGlobalCooldownAsync(command, cancellationToken);

            if (isOnGlobalCooldown)
            {
                return CommandAvailability.OnGlobalCooldown;
            }

            var isOnViewerCooldown = await DetermineCommandIsOnViewerCooldownAsync(command, senderUsername, cancellationToken);

            if (isOnViewerCooldown)
            {
                return CommandAvailability.OnViewerCooldown;
            }
        }

        return CommandAvailability.Available;
    }

    private async Task<bool> DetermineCommandIsOnGlobalCooldownAsync(Command commandToCheck, CancellationToken cancellationToken)
    {
        // check if command is on global cooldown for stream by looking up relevant key
        var cooldownKey = GlobalCooldownKey(commandToCheck.Id);

        return await _cacheService.ReadValueAsync<bool>(cooldownKey, cancellationToken);
    }

    private async Task<bool> DetermineCommandIsOnViewerCooldownAsync(Command command, string senderUsername, CancellationToken cancellationToken)
    {
        var cooldownKey = ViewerCooldownKey(command.Id, senderUsername);

        return await _cacheService.ReadValueAsync<bool>(cooldownKey, cancellationToken);
    }

    private async Task ActivateCommandCooldownsAsync(Command command, string senderUsername, CancellationToken cancellationToken)
    {
        await _cacheService.WriteValueAsync(GlobalCooldownKey(command.Id), true,
            TimeSpan.FromSeconds(command.GlobalCooldownSeconds), true, cancellationToken);

        await _cacheService.WriteValueAsync(ViewerCooldownKey(command.Id, senderUsername), true,
            TimeSpan.FromSeconds(command.GlobalCooldownSeconds), true, cancellationToken);
    }

    private static string GlobalCooldownKey(long commandId)
        => ConstructCooldownKey(commandId, GlobalCooldownIdentifier);

    private static string ViewerCooldownKey(long commandId, string senderUsername)
        => ConstructCooldownKey(commandId, senderUsername);

    private static string ConstructCooldownKey(long commandId, string cooldownIdentifier)
        => string.Join('-', commandId, "cooldown", cooldownIdentifier);
}