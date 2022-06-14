using MediatR;
using StreamInstruments.DataObjects;

namespace StreamInstruments.Hubs.Api.SecondaryPorts.GetCommandsFromDatabase;

public record GetCommandsFromDatabaseQuery : IRequest<List<Command>>;