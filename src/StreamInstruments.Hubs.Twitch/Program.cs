using StreamInstruments.DataAccess.Services;
using StreamInstruments.Hubs.Commands.Domain.Infrastructure;
using StreamInstruments.Hubs.Commands.Infrastructure.Infrastructure;
using StreamInstruments.Hubs.Twitch;
using StreamInstruments.Services.Caching.Installers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        DataAccessInstaller.Install(services);
        CachingInstaller.Install(services);
        DomainInstaller.Install(services);
        InfrastructureInstaller.Install(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();