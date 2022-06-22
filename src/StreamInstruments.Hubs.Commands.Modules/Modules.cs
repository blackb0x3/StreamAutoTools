using System;

namespace StreamInstruments.Hubs.Commands.Modules;

[Flags]
public enum Modules
{
    Dummy, // for testing purposes, not to be used by streams!
    Variable,
    CustomApi,
    Coin,
    Dice,
    RockPaperScissors,
    RockPaperScissorsLizardSpock,
}