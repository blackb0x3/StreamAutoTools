namespace StreamInstruments.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Extracts a substring between the first occurrence of <see cref="start"/>
    /// and the last occurrence of <see cref="end"/>.
    /// </summary>
    /// <param name="toExtractFrom">The string to extract from.</param>
    /// <param name="start">Where to start extracting from in the string <see cref="toExtractFrom"/></param>
    /// <param name="end">Where to stop extracting from in the string <see cref="toExtractFrom"/></param>
    /// <returns>
    /// The substring between the first occurrence of <see cref="start"/> and the last occurrence of <see cref="end"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// When either the <see cref="start"/> or <see cref="end"/> string does not appear in the
    /// string <see cref="toExtractFrom"/>.
    /// </exception>
    public static string ExtractSubstring(this string toExtractFrom, string start, string end)
    {
        // quick exit here, as it's the equivalent of returning the input string when true
        if (string.IsNullOrEmpty(start) && string.IsNullOrEmpty(end))
        {
            return toExtractFrom;
        }

        var startIndex = toExtractFrom.IndexOf(start, StringComparison.Ordinal);
        var endIndex = toExtractFrom.LastIndexOf(end, StringComparison.Ordinal);

        if (startIndex < 0)
        {
            throw new ArgumentException($"'{start}' does not appear in string '{toExtractFrom}'.");
        }

        if (endIndex < 0)
        {
            throw new ArgumentException($"'{end}' does not appear in string '{toExtractFrom}'.");
        }

        if (startIndex > endIndex)
        {
            throw new ArgumentException($"{nameof(startIndex)} cannot be ahead of {nameof(endIndex)}.");
        }

        var startExtractingFromIndex = startIndex + start.Length;

        return toExtractFrom[startExtractingFromIndex..endIndex];
    }
}