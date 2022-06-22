using System;
using System.Threading.Tasks;

namespace StreamInstruments.Hubs.Commands.Modules.ModuleImplementations;

public class CoinModule : ModuleBase, IModule
{
    private const string HeadsLabel = "Heads";
    private const string TailsLabel = "Tails";

    public override Modules Type => Modules.Coin;

    public string Toss()
    {
        return Rng.NextDouble() >= 0.5 ? HeadsLabel : TailsLabel;
    }

    public string Heads()
    {
        return HeadsLabel;
    }

    public string Tails()
    {
        return TailsLabel;
    }
}