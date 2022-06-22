using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StreamInstruments.Hubs.Commands.Modules.IoC;

namespace StreamInstruments.Hubs.Commands.Domain.Infrastructure;

public static class DomainInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddMediatR(DomainAssembly);
        ModulesInstaller.Install(services);
    }

    private static Assembly DomainAssembly => typeof(DomainInstaller).Assembly;
}