using AutoMapper;
using StreamInstruments.DataObjects;
using StreamInstruments.Extensions;

namespace StreamInstruments.Hubs.Api.Domain.Mapping.Converters;

public class CommandEntityToCommandRepresentationCommandConverter : ITypeConverter<Command, Representations.Command>
{
    public Representations.Command Convert(Command source, Representations.Command destination, ResolutionContext context)
    {
        return new Representations.Command
        {
            Name = source.Name,
            ResponseText = source.ResponseText,
            AccessLevel = source.AccessLevel.ToDescription(),
            ResponseDestination = source.ResponseDestination.ToDescription(),
            IndividualCooldownSeconds = source.IndividualCooldownSeconds,
            GlobalCooldownSeconds = source.GlobalCooldownSeconds,
            IsActive = source.IsActive ? "Yes" : "No"
        };
    }
}