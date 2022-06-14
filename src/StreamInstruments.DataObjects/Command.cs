namespace StreamInstruments.DataObjects;

public class Command : EntityBase
{
    /// <summary>
    /// The name of the command the user types into Twitch chat. Does not require the ! symbol at the start.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Some useful information about the command. E.g. what it does, usage, 
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The pre-processed response text sent to the specified <see cref="ResponseDestination"/>.
    /// </summary>
    public string ResponseText { get; set; }

    /// <summary>
    /// The allowed access level of this command.
    /// </summary>
    public ViewerLevel AccessLevel { get; set; }

    /// <summary>
    /// The destination of the response from running this command.
    /// </summary>
    public ResponseDestination ResponseDestination { get; set; }

    /// <summary>
    /// Allowed interval between running this command. User-Specific.
    /// </summary>
    public int IndividualCooldownSeconds { get; set; }

    /// <summary>
    /// Allowed interval between running this command. Applies to all users.
    /// </summary>
    public int GlobalCooldownSeconds { get; set; }

    /// <summary>
    /// Determines if the command is available to use if detected in Twitch chat.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Determines if the command is marked as deleted. Allows for easy undo in the future.
    /// </summary>
    public bool IsDeleted { get; set; }
}