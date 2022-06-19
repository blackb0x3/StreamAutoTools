namespace StreamInstruments.Hubs.Commands.Infrastructure.Helpers;

public class CooldownKeyHelper
{
    public static string ConstructCooldownKey(long commandId, string cooldownIdentifier)
        => string.Join('-', commandId, "cooldown", cooldownIdentifier);
}