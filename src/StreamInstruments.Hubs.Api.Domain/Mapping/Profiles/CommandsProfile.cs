using AutoMapper;
using StreamInstruments.DataObjects;
using StreamInstruments.Hubs.Api.Domain.Mapping.Converters;

namespace StreamInstruments.Hubs.Api.Domain.Mapping.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Command, Representations.Command>()
            .ConvertUsing<CommandEntityToCommandRepresentationConverter>();
    }
}