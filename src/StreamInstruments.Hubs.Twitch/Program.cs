using Serilog;
using StreamInstruments.DataAccess.Services;
using StreamInstruments.Hubs.Commands.Domain.Infrastructure;
using StreamInstruments.Hubs.Commands.Infrastructure.Infrastructure;
using StreamInstruments.Hubs.Twitch;
using StreamInstruments.Logging;
using StreamInstruments.Services.Caching.Installers;

LoggerGenerator.SetupLogger();

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
    .ConfigureServices(services =>
    {
        DataAccessInstaller.Install(services);
        CachingInstaller.Install(services);
        LoggingInstaller.Install(services);
        DomainInstaller.Install(services);
        InfrastructureInstaller.Install(services);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();