using MediatR;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandAvailability;

namespace StreamInstruments.Hubs.Commands.Infrastructure.Adapters;

public class GetCommandAvailabilityQueryHandler : IRequestHandler<GetCommandAvailabilityQuery, GetCommandAvailabilityResponse>
{
    public Task<GetCommandAvailabilityResponse> Handle(GetCommandAvailabilityQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}