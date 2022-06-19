using MediatR;
using StreamInstruments.DataObjects;
using StreamInstruments.Models;

namespace StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;

public class GetCommandAvailabilityQuery : IRequest<GetCommandAvailabilityResponse>
{
    public Command Command { get; set; }

    public string SenderUsername { get; set; }

    public StreamingService StreamingService { get; set; }
}