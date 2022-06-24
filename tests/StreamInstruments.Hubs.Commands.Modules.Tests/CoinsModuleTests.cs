using FluentAssertions;
using NUnit.Framework;
using StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

namespace StreamInstruments.Hubs.Commands.Modules.Tests;

[TestFixture]
public class A_CoinModule
{
    [Test]
    public void Returns_Heads_Or_Tails()
    {
        var module = new CoinModule();

        module.Toss().ToLower().Should().BeOneOf("heads", "tails");
    }
}