namespace StreamInstruments.Hubs.Api.Domain.Representations;

public class CommandsRepresentation
{
    public List<Command> Commands { get; set; }
}

public class Command
{
    public string Name { get; set; }

    public string ResponseText { get; set; }

    public string AccessLevel { get; set; }

    public string ResponseDestination { get; set; }

    public int IndividualCooldownSeconds { get; set; }

    public int GlobalCooldownSeconds { get; set; }

    public string IsActive { get; set; }
}