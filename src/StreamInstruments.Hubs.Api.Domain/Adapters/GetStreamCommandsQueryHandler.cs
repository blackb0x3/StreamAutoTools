using AutoMapper;
using MediatR;
using OneOf;
using StreamInstruments.Hubs.Api.Domain.PrimaryPorts.GetStreamCommands;
using StreamInstruments.Hubs.Api.Domain.Representations;
using StreamInstruments.Hubs.Api.SecondaryPorts.GetCommandsFromDatabase;

namespace StreamInstruments.Hubs.Api.Domain.Adapters;

internal class GetStreamCommandsQueryHandler : IRequestHandler<GetStreamCommandsQuery, OneOf<GetStreamCommandsResponse, ErrorResponse>>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public GetStreamCommandsQueryHandler(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<OneOf<GetStreamCommandsResponse, ErrorResponse>> Handle(GetStreamCommandsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var getCommandsFromDbQuery = new GetCommandsFromDatabaseQuery();
            var commandEntities = await _mediator.Send(getCommandsFromDbQuery, cancellationToken);

            return new GetStreamCommandsResponse
            {
                CommandsRepresentation = new CommandsRepresentation
                {
                    Commands = commandEntities.Select(cmd => _mapper.Map<Command>(cmd)).ToList()
                }
            };
        }
        catch (Exception e)
        {
            return new ErrorResponse { Message = e.Message };
        }
    }
}