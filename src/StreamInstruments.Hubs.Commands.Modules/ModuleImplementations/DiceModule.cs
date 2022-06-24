using System.Globalization;

namespace StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

public class DiceModule : ModuleBase, IModule
{
    public override Modules Type => Modules.Dice;

    public string Roll(string min, string max, string timesToRoll)
    {
        var numbersRolled = new List<int>();
        var parsedMin = int.TryParse(min, NumberStyles.Integer, CultureInfo.InvariantCulture, out var minInt);
        var parsedMax = int.TryParse(max, NumberStyles.Integer, CultureInfo.InvariantCulture, out var maxInt);
        var timesToRollParsed = int.TryParse(timesToRoll, NumberStyles.Integer, CultureInfo.InvariantCulture, out var timesToRollInt);

        if (!parsedMin)
        {
            throw new Exception($"Invalid lowest dice number - {min}");
        }

        if (!parsedMax)
        {
            throw new Exception($"Invalid highest dice number - {max}");
        }

        if (!timesToRollParsed)
        {
            throw new Exception($"Invalid number of die to roll - {timesToRoll}");
        }

        if (timesToRollInt < 1)
        {
            throw new Exception($"Number of die to roll must be greater than 0. (Found {timesToRoll})");
        }

        if (minInt > maxInt)
        {
            throw new Exception($"Highest number (found {maxInt}) must be greater than lowest number (found {minInt})");
        }

        for (var i = 0; i < timesToRollInt; i++)
        {
            numbersRolled.Add(Rng.Next(minInt, maxInt));
        }

        return string.Join(", ", numbersRolled);
    }

    public string RollOnce(string min, string max)
    {
        return Roll(min, max, "1");
    }

    public string RollTwice(string min, string max)
    {
        return Roll(min, max, "2");
    }
}