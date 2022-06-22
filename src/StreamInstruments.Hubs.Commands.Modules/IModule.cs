using System.Collections.Generic;
using System.Threading.Tasks;

namespace StreamInstruments.Hubs.Commands.Modules;

public interface IModule
{
    Modules Type { get; }

    Task<string> ExecuteFunctionAsync(string functionName, string[] functionParams);
}