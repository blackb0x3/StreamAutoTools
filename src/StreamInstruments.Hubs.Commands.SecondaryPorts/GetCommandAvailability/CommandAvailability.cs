namespace StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;

public enum CommandAvailability
{
    Available,
    NotAvailable,
    AccessRestricted,
    OnViewerCooldown,
    OnGlobalCooldown
}