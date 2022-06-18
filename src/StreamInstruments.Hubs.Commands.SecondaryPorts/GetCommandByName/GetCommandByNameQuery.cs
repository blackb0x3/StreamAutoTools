using MediatR;
using StreamInstruments.DataObjects;

namespace StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandByName;

public class GetCommandByNameQuery : IRequest<Command?>
{
    public string CommandName { get; set; }
}