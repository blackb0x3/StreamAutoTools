using MediatR;
using OneOf;
using StreamInstruments.DataObjects;
using StreamInstruments.Hubs.Commands.Domain.PrimaryPorts.ExecuteCommand;
using StreamInstruments.Hubs.Commands.Domain.Representations;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandByName;

namespace StreamInstruments.Hubs.Commands.Domain.Adapters;

internal class ExecuteCommandRequestHandler : IRequestHandler<ExecuteCommandRequest, OneOf<ExecuteCommandResponse, ErrorResponse>>
{
    private readonly IMediator _mediator;

    public ExecuteCommandRequestHandler(IMediator mediator)
    {
        _mediator = mediator;
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
            var commandAvailabilityResp = await GetCommandAvailabilityAsync(command, request.SenderUsername);

            if (!commandAvailabilityResp.CommandCanBeUsed)
            {
                return new ErrorResponse
                {
                    // There's no point sending a message if command shouldn't be run. The whole point of
                    // cooldown, access levels etc. is to mitigate bot messages flooding chat for no reason.
                    Message = $"Command {request.CommandName} cannot be used. Reason: {commandAvailabilityResp.Reason}"
                };
            }
        }

        // Parse the command text

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

    private async Task<GetCommandAvailabilityResponse> GetCommandAvailabilityAsync(Command command, string senderUsername)
    {
        var getCommandAvailabilityQuery = new GetCommandAvailabilityQuery
        {
            CommandName = command.Name,
            SenderUsername = senderUsername
        };

        return await _mediator.Send(getCommandAvailabilityQuery);
    }
}