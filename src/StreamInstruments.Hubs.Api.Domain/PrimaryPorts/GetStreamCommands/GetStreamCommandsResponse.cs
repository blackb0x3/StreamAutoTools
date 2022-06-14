using StreamInstruments.Hubs.Api.Domain.Representations;

namespace StreamInstruments.Hubs.Api.Domain.PrimaryPorts.GetStreamCommands;

public class GetStreamCommandsResponse
{
    public CommandsRepresentation CommandsRepresentation { get; set; }
}