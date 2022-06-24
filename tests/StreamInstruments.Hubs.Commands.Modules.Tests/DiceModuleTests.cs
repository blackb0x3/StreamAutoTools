using System.Globalization;
using FluentAssertions;
using NUnit.Framework;
using StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

namespace StreamInstruments.Hubs.Commands.Modules.Tests;

[TestFixture]
public class A_DiceModule
{
    private readonly DiceModule _module = new();

    [Test]
    [TestCase(1, 6, 1)]
    [TestCase(1, 100, 50)]
    [TestCase(-9, -2, 6)]
    public void Rolls_X_Number_Of_Times(int min, int max, int timesToRoll)
    {
        var result = _module.Roll(min.ToString(), max.ToString(), timesToRoll.ToString());
        var numbers = result.Split(", ");

        numbers.Should().HaveCount(timesToRoll);

        foreach (var num in numbers)
        {
            int.TryParse(num, out var parsed)
                .Should().BeTrue();

            parsed.Should().BeInRange(min, max);
        }
    }

    [Test]
    [TestCase("abc")]
    [TestCase("g^JyRe%gH£bt_rH&")]
    [TestCase("sixty nine")]
    [TestCase("")]
    [TestCase("7.5")]
    [TestCase("3.14159265")]
    [TestCase("2.00")]
    public void Throws_Exception_When_Min_Is_Not_An_Integer(string min)
    {
        Action act = () => { _module.Roll(min, "10", "1"); };

        act.Should().Throw<Exception>().WithMessage($"Invalid lowest dice number - {min}");
    }

    [Test]
    [TestCase("abc")]
    [TestCase("g^JyRe%gH£bt_rH&")]
    [TestCase("sixty nine")]
    [TestCase("")]
    [TestCase("7.5")]
    [TestCase("3.14159265")]
    [TestCase("2.00")]
    public void Throws_Exception_When_Max_Is_Not_An_Integer(string max)
    {
        Action act = () => { _module.Roll("1", max, "1"); };

        act.Should().Throw<Exception>().WithMessage($"Invalid highest dice number - {max}");;
    }

    [Test]
    [TestCase("abc")]
    [TestCase("g^JyRe%gH£bt_rH&")]
    [TestCase("sixty nine")]
    [TestCase("")]
    [TestCase("7.5")]
    [TestCase("3.14159265")]
    [TestCase("2.00")]
    public void Throws_Exception_When_TimesToRoll_Is_Not_A_Positive_Integer(string timesToRoll)
    {
        Action act = () => { _module.Roll("1", "10", timesToRoll); };

        act.Should().Throw<Exception>().WithMessage($"Invalid number of die to roll - {timesToRoll}");
    }

    [Test]
    [TestCase("-5")]
    [TestCase("0")]
    public void Throws_Exception_When_TimesToRoll_Is__Less_Than_Or_Equal_To_Zero(string timesToRoll)
    {
        Action act = () => { _module.Roll("1", "10", timesToRoll); };

        act.Should().Throw<Exception>().WithMessage($"Number of die to roll must be greater than 0. (Found {timesToRoll})");
    }

    [Test]
    public void Throws_Exception_When_Max_Is_Greater_Than_Min()
    {
        Action act = () => { _module.Roll("10", "1", "1"); };

        act.Should().Throw<Exception>()
            .WithMessage($"Highest number (found 1) must be greater than lowest number (found 10)");
    }
}