using System.ComponentModel;
using System.Reflection;

namespace StreamInstruments.Extensions;

public static class EnumExtensions
{
    public static string ToDescription(this Enum val)
    {
        FieldInfo? fi = val.GetType().GetField(val.ToString());

        if (fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
        {
            return attributes.First().Description;
        }

        return val.ToString();
    }
}