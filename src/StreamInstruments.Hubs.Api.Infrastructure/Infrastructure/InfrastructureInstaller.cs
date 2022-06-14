using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace StreamInstruments.Hubs.Api.Infrastructure.Infrastructure;

public static class InfrastructureInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddMediatR(typeof(InfrastructureInstaller).Assembly);
    }
}