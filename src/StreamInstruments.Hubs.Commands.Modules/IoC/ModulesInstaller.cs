using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace StreamInstruments.Hubs.Commands.Modules.IoC;

public static class ModulesInstaller
{
    public static void Install(IServiceCollection services)
    {
        RegisterModules(services);

        services.AddTransient<IModuleFactory, ModuleFactory>();
    }

    private static void RegisterModules(IServiceCollection services)
    {
        var moduleInterfaceType = typeof(IModule);
        var moduleImpls = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => moduleInterfaceType.IsAssignableFrom(type) && !type.IsInterface);

        foreach (var module in moduleImpls)
        {
            services.AddTransient(moduleInterfaceType, module);
        }
    }
}