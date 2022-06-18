using MediatR;
using OneOf;
using StreamInstruments.Hubs.Commands.Domain.Representations;

namespace StreamInstruments.Hubs.Commands.Domain.PrimaryPorts.ExecuteCommand;

public class ExecuteCommandRequest : IRequest<OneOf<ExecuteCommandResponse, ErrorResponse>>
{
    public string CommandName { get; set; }

    public bool IsDryRun { get; set; }

    public string SenderUsername { get; set; }
}