using StreamInstruments.DataObjects;

namespace StreamInstruments.Hubs.Commands.Domain.PrimaryPorts.ExecuteCommand;

public class ExecuteCommandResponse
{
    public string Output { get; set; }

    public ResponseDestination ResponseDestination { get; set; }
}