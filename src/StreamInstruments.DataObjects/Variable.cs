namespace StreamInstruments.DataObjects;

public class Variable : EntityBase
{
    /// <summary>
    /// The name of the variable. Accessed by <see cref="Command"/> syntax.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the variable. Purely for editor purposes.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The type of information stored in this variable.
    /// </summary>
    public VariableType Type { get; set; }

    /// <summary>
    /// The raw value of this variable, encoded as a string.
    /// </summary>
    public string RawValue { get; set; }

    /// <summary>
    /// Determines if this variable is enabled for use in commands.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Determines if this variable is marked as deleted. Allows for easy undo in the future.
    /// </summary>
    public bool IsDeleted { get; set; }
}