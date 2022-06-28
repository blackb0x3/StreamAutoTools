using StreamInstruments.Logging;

namespace StreamInstruments.Hubs.Twitch;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInfo(new { msg = $"Worker running at: {DateTimeOffset.Now}" });
            await Task.Delay(1000, stoppingToken);
        }
    }
}