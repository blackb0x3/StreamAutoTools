using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace StreamInstruments.Hubs.Commands.Infrastructure.Infrastructure;

public static class InfrastructureInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddMediatR(DomainAssembly);
    }

    private static Assembly DomainAssembly => typeof(InfrastructureInstaller).Assembly;
}