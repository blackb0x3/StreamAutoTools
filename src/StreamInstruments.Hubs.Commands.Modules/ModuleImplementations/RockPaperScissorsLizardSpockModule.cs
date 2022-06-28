using System;

namespace StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

public class RockPaperScissorsLizardSpockModule : ModuleBase, IModule
{
    private const string RockLabel = "rock";
    private const string PaperLabel = "paper";
    private const string ScissorsLabel = "scissors";
    private const string LizardLabel = "lizard";
    private const string SpockLabel = "spock";

    private const string ExplanationUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fe/Rock_Paper_Scissors_Lizard_Spock_en.svg/1024px-Rock_Paper_Scissors_Lizard_Spock_en.svg.png";

    public override Modules Type => Modules.RockPaperScissorsLizardSpock;

    public string PickRandom(string givenChoice)
    {
        givenChoice = givenChoice.ToLower();
        ValidatePlayerChoice(givenChoice);
        var rng = new Random().NextDouble();

        var cpuChoice = rng switch
        {
            > 0 and < 0.2 => RockLabel,
            > 0.2 and < 0.4 => PaperLabel,
            > 0.4 and < 0.6 => ScissorsLabel,
            > 0.6 and < 0.8 => LizardLabel,
            _ => SpockLabel
        };

        return Output(givenChoice, cpuChoice);
    }

    public string CounterPick(string givenChoice)
    {
        givenChoice = givenChoice.ToLower();
        ValidatePlayerChoice(givenChoice);
        var rng = new Random().NextDouble();

        var cpuChoice = givenChoice switch
        {
            RockLabel => rng <= 0.5 ? PaperLabel : SpockLabel,
            PaperLabel => rng <= 0.5 ? ScissorsLabel : LizardLabel,
            ScissorsLabel => rng <= 0.5 ? SpockLabel : RockLabel,
            SpockLabel => rng <= 0.5 ? LizardLabel : PaperLabel,
            // should never reach here, but putting it here otherwise, Rider throws a hissy fit
            _ => throw new ArgumentOutOfRangeException(nameof(givenChoice), givenChoice, null)
        };

        return Output(givenChoice, cpuChoice);
    }

    public string Explanation()
    {
        return ExplanationUrl;
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
            ? "It's a tie! PogChamp"
            : PlayerWins(playerChoice, cpuChoice)
                ? "You win!"
                : "You lose!";

        return $"I chose {cpuChoice}, you chose {playerChoice}. {winConditionMessage}";
    }

    private static bool PlayerWins(string playerChoice, string cpuChoice) =>
        (playerChoice == ScissorsLabel && cpuChoice == PaperLabel)  // scissors cuts paper
        || (playerChoice == PaperLabel && cpuChoice == RockLabel)      // paper covers rock
        || (playerChoice == RockLabel && cpuChoice == LizardLabel)     // rock crushes lizard
        || (playerChoice == LizardLabel && cpuChoice == SpockLabel)    // lizard poisons spock
        || (playerChoice == SpockLabel && cpuChoice == ScissorsLabel)  // spock destroys scissors
        || (playerChoice == ScissorsLabel && cpuChoice == LizardLabel) // scissors decapitates lizard
        || (playerChoice == LizardLabel && cpuChoice == PaperLabel)    // lizard eats paper
        || (playerChoice == PaperLabel && cpuChoice == SpockLabel)     // paper disproves spock
        || (playerChoice == SpockLabel && cpuChoice == RockLabel)      // spock vaporizes rock
        || (playerChoice == RockLabel && cpuChoice == ScissorsLabel);  // rock breaks scissors
}