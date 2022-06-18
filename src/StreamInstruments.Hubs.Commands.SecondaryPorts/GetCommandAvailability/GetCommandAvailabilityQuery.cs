using MediatR;

namespace StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;

public class GetCommandAvailabilityQuery : IRequest<GetCommandAvailabilityResponse>
{
    public string CommandName { get; set; }

    public string SenderUsername { get; set; }
}