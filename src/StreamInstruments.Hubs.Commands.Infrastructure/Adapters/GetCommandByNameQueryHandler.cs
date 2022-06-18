using MediatR;
using Microsoft.EntityFrameworkCore;
using StreamInstruments.DataAccess;
using StreamInstruments.DataObjects;
using StreamInstruments.Hubs.Commands.SecondaryPorts.GetCommandByName;

namespace StreamInstruments.Hubs.Commands.Infrastructure.Adapters;

internal class GetCommandByNameQueryHandler : IRequestHandler<GetCommandByNameQuery, Command?>
{
    private readonly StreamInstrumentsContext _context;

    public GetCommandByNameQueryHandler(StreamInstrumentsContext context)
    {
        _context = context;
    }

    public async Task<Command?> Handle(GetCommandByNameQuery request, CancellationToken cancellationToken)
    {
        return await _context.Commands.FirstOrDefaultAsync(
            cmd => string.Equals(request.CommandName, cmd.Name, StringComparison.OrdinalIgnoreCase),
            cancellationToken: cancellationToken);
    }
}