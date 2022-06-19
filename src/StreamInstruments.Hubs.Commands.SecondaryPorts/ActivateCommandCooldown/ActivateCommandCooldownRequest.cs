using MediatR;
using StreamInstruments.DataObjects;

namespace StreamInstruments.Hubs.Commands.SecondaryPorts.ActivateCommandCooldown;

public class ActivateCommandCooldownRequest : IRequest<Unit>
{
    public long CommandId { get; set; }

    public int CooldownSeconds { get; set; }

    public string CooldownIdentifier { get; set; }
}