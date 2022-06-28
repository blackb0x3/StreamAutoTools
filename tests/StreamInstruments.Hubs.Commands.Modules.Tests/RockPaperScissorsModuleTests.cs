using FluentAssertions;
using NUnit.Framework;
using StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

namespace StreamInstruments.Hubs.Commands.Modules.Tests;

[TestFixture]
public class A_RockPaperScissorsModule
{
    private readonly RockPaperScissorsModule _module = new();

    [Test]
    [TestCase("rock")]
    [TestCase("paper")]
    [TestCase("scissors")]
    [TestCase("ROCK")]
    [TestCase("PaPeR")]
    [TestCase("ScIsSoRs")]
    public void Returns_Appropriate_Result_On_PickRandom(string playerChoice)
    {
        var result = _module.PickRandom(playerChoice);

        // "I chose <x>"
        var cpuChoice = result.Split(",")[0].Split().Last();

        // trim the whitespace after the period
        var winConditionMessage = result.Split(". ").Last();

        cpuChoice.Should().BeOneOf("rock", "paper", "scissors");
        winConditionMessage.Should().BeOneOf("It's a tie!", "You win!", "You lose!");
    }

    [Test]
    [TestCase("rock", "paper")]
    [TestCase("paper", "scissors")]
    [TestCase("scissors", "rock")]
    public void Returns_The_Appropriate_Counter_On_CounterPick(string playerChoice, string expectedCpuChoice)
    {
        var result = _module.CounterPick(playerChoice);

        // "I chose <x>"
        var cpuChoice = result.Split(",")[0].Split().Last();

        // trim the whitespace after the period
        var winConditionMessage = result.Split(". ").Last();

        cpuChoice.Should().Be(expectedCpuChoice);
        winConditionMessage.Should().Be("You lose!");
    }

    public void Throws_Exception_On_Invalid_Player_Choice()
    {
        Action act = () => { _module.CounterPick("SPOON"); };

        act.Should().Throw<Exception>().WithMessage("Unknown option SPOON");
    }
}