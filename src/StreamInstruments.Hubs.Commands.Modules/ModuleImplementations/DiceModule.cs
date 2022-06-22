namespace StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

public class DiceModule : ModuleBase, IModule
{
    public override Modules Type => Modules.Dice;

    public string Roll(string min, string max, string timesToRoll)
    {
        var numbersRolled = new List<int>();
        var minInt = Convert.ToInt32(min);
        var maxInt = Convert.ToInt32(max);
        var timesToRollInt = Convert.ToInt32(timesToRoll);

        for (var i = 0; i < timesToRollInt; i++)
        {
            numbersRolled.Add(Rng.Next(minInt, maxInt));
        }

        return string.Join(", ", numbersRolled);
    }

    public string RollOnce(string min, string max)
    {
        var minInt = Convert.ToInt32(min);
        var maxInt = Convert.ToInt32(max);

        return Rng.Next(minInt, maxInt).ToString();
    }

    public string RollTwice(string min, string max)
    {
        var minInt = Convert.ToInt32(min);
        var maxInt = Convert.ToInt32(max);
        var first = Rng.Next(minInt, maxInt).ToString();
        var second = Rng.Next(minInt, maxInt).ToString();

        return string.Join(",", first, second);
    }
}