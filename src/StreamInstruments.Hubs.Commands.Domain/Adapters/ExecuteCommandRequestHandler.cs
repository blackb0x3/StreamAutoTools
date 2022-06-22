using System.Text;
using MediatR;
using OneOf;
using PCRE;
using StreamInstruments.DataObjects;
using StreamInstruments.Extensions;
using StreamInstruments.Hubs.Commands.Domain.PrimaryPorts.ExecuteCommand;
using StreamInstruments.Hubs.Commands.Domain.Representations;
using StreamInstruments.Hubs.Commands.SecondaryPorts.ActivateCommandCooldown;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandByName;
using StreamInstruments.Interfaces;

namespace StreamInstruments.Hubs.Commands.Domain.Adapters;

internal class ExecuteCommandRequestHandler : IRequestHandler<ExecuteCommandRequest, OneOf<ExecuteCommandResponse, ErrorResponse>>
{
    /// <remarks>
    /// PcreRegex supports recursive regex tokens, which are required by the handler to
    /// find command text expressions.
    /// Command text expressions are of the form: ${module.function arg1 arg2 arg3 etc.}
    /// </remarks>
    private static readonly PcreRegex _commandExpressionRegex = new(@"(\$\{(?>[^${}]+|(?1))+\})");

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

        // early exit if command doesn't exist
        if (getCommandResp.IsT1)
        {
            return getCommandResp.AsT1;
        }

        var command = getCommandResp.AsT0;

        // If request is not a dry run
        //    - check for command availability (right access level, no cooldown etc.)
        if (!request.IsDryRun)
        {
            var commandAvailabilityQuery = new GetCommandAvailabilityQuery
            {
                Command = command,
                SenderUsername = request.SenderUsername,
                StreamingService = request.StreamingService
            };

            var commandAvailabilityResp = await _mediator.Send(commandAvailabilityQuery, cancellationToken);

            // There's no point sending a message if command shouldn't be run.
            // The whole point of command availability (cooldown, access levels etc.)
            // is to mitigate bot messages flooding chat for no reason.
            if (commandAvailabilityResp.CommandAvailability != CommandAvailability.Available)
            {
                return new ErrorResponse
                {
                    Message = $"Command {request.CommandName} cannot be used. Reason: {commandAvailabilityResp.CommandAvailability.ToDescription()}"
                };
            }
        }

        // Parse the command text
        var parsedCommandText = await ParseCommandTextAsync(command.ResponseText, request.MessageArguments);

        // Before returning - ensure we activate the cooldowns
        var activateGlobalCooldownRequest = new ActivateCommandCooldownRequest
        {
            CommandId = command.Id,
            CooldownSeconds = command.GlobalCooldownSeconds,
            CooldownIdentifier = CacheKeyConstants.GlobalCooldownIdentifier
        };

        var activateViewerCooldownRequest = new ActivateCommandCooldownRequest
        {
            CommandId = command.Id,
            CooldownSeconds = command.IndividualCooldownSeconds,
            CooldownIdentifier = request.SenderUsername
        };

        var globalCooldownTask = _mediator.Send(activateGlobalCooldownRequest, cancellationToken);
        var viewerCooldownTask = _mediator.Send(activateViewerCooldownRequest, cancellationToken);

        await Task.WhenAll(globalCooldownTask, viewerCooldownTask);

        // Return a response which includes the parsed command text
        return new ExecuteCommandResponse
        {
            Output = parsedCommandText,
            ResponseDestination = command.ResponseDestination
        };
    }

    private async Task<string> ParseCommandTextAsync(string commandText, string[] messageArgs)
    {
        var sb = new StringBuilder(commandText);

        var commandExpressionMatches = _commandExpressionRegex.Matches(commandText);

        foreach (var match in commandExpressionMatches)
        {
            // for each match, there is guaranteed to be a single group containing a matching non-null, non-empty string
            //     - the command expression regex checks this for us
            var matchText = match.Groups
                .First().Value
                .ExtractSubstring("${", "}");
        }

        return sb.ToString();
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
}