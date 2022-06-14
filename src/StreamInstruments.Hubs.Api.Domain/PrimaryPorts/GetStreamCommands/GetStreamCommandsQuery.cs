using MediatR;
using OneOf;
using StreamInstruments.Hubs.Api.Domain.Representations;

namespace StreamInstruments.Hubs.Api.Domain.PrimaryPorts.GetStreamCommands;

public record GetStreamCommandsQuery : IRequest<OneOf<GetStreamCommandsResponse, ErrorResponse>>;