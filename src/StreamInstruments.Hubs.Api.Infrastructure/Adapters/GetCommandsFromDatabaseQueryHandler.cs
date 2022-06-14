using MediatR;
using Microsoft.EntityFrameworkCore;
using StreamInstruments.DataAccess;
using StreamInstruments.DataObjects;
using StreamInstruments.Hubs.Api.SecondaryPorts.GetCommandsFromDatabase;

namespace StreamInstruments.Hubs.Api.Infrastructure.Adapters;

internal class GetCommandsFromDatabaseQueryHandler : IRequestHandler<GetCommandsFromDatabaseQuery, List<Command>>
{
    private readonly StreamInstrumentsContext _context;

    public GetCommandsFromDatabaseQueryHandler(StreamInstrumentsContext context)
    {
        _context = context;
    }

    public async Task<List<Command>> Handle(GetCommandsFromDatabaseQuery request, CancellationToken cancellationToken)
    {
        return await _context.Commands
            .AsQueryable()
            .Where(cmd => !cmd.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}