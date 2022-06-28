using System;

namespace StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

public class RockPaperScissorsModule : ModuleBase, IModule
{
    private const string RockLabel = "rock";
    private const string PaperLabel = "paper";
    private const string ScissorsLabel = "scissors";

    public override Modules Type => Modules.RockPaperScissors;

    public string PickRandom(string givenChoice)
    {
        givenChoice = givenChoice.ToLower();
        ValidatePlayerChoice(givenChoice);
        var rng = new Random().NextDouble();

        var cpuChoice = rng switch
        {
            > 0 and <= 0.3333333 => RockLabel,
            > 0.3333333 and <= 0.6666667 => PaperLabel,
            _ => ScissorsLabel
        };

        return Output(givenChoice, cpuChoice);
    }

    public string CounterPick(string givenChoice)
    {
        givenChoice = givenChoice.ToLower();
        ValidatePlayerChoice(givenChoice);

        var counterPick = givenChoice switch
        {
            RockLabel => PaperLabel,
            PaperLabel => ScissorsLabel,
            ScissorsLabel => RockLabel,
            // should never reach here, but putting it here otherwise, Rider throws a hissy fit
            _ => throw new ArgumentOutOfRangeException(nameof(givenChoice), givenChoice, null)
        };

        return Output(givenChoice, counterPick);
    }

    private static void ValidatePlayerChoice(string playerChoice)
    {
        var validChoices = new List<string> { RockLabel, PaperLabel, ScissorsLabel };

        if (!validChoices.Contains(playerChoice))
        {
            throw new Exception($"Unknown option {playerChoice}");
        }
    }

    private static string Output(string playerChoice, string cpuChoice)
    {
        var winConditionMessage = playerChoice == cpuChoice
            ? "It's a tie!"
            : PlayerWins(playerChoice, cpuChoice)
                ? "You win!"
                : "You lose!";

        return $"I chose {cpuChoice}, you chose {playerChoice}. {winConditionMessage}";
    }

    private static bool PlayerWins(string playerChoice, string cpuChoice) =>
        (playerChoice == ScissorsLabel && cpuChoice == PaperLabel)
        || (playerChoice == PaperLabel && cpuChoice == RockLabel)
        || (playerChoice == RockLabel && cpuChoice == ScissorsLabel);
}