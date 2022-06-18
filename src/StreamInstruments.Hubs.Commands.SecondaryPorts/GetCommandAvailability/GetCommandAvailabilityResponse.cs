namespace StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;

public class GetCommandAvailabilityResponse
{
    public bool CommandCanBeUsed { get; set; }

    public string Reason { get; set; }
}