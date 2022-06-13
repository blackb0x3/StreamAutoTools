namespace StreamInstruments.DataObjects;

public enum VariableType
{
    /// <summary>
    /// A custom type.
    /// </summary>
    Custom,
    /// <summary>
    /// A text value.
    /// </summary>
    Text,
    /// <summary>
    /// A numeric integer value.
    /// </summary>
    Number,
    /// <summary>
    /// A numeric floating point value.
    /// </summary>
    Decimal,
    /// <summary>
    /// A True / False (i.e. yes / no) value.
    /// </summary>
    Boolean
}