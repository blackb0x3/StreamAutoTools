using System;
using System.Collections.Generic;

namespace StreamInstruments.Hubs.Commands.Modules;

public class ModuleFactory : IModuleFactory
{
    private readonly IDictionary<Modules, IModule> _modules;

    public ModuleFactory(IEnumerable<IModule> modules)
    {
        _modules = new Dictionary<Modules, IModule>();

        foreach (var module in modules)
        {
            _modules[module.Type] = module;
        }
    }

    public IModule GetModule(string moduleName)
    {
        if (!Enum.TryParse<Modules>(moduleName, true, out var parsedModule))
        {
            throw new Exception($"Unknown module `{moduleName}`!");
        }

        return _modules[parsedModule];
    }
}