namespace StreamInstruments.Hubs.Commands.Domain.Representations;

public enum CommandAvailability
{
    Available,
    NotAvailable,
    AccessRestricted,
    OnViewerCooldown,
    OnGlobalCooldown
}