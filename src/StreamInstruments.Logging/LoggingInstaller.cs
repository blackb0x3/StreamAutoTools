using Microsoft.Extensions.DependencyInjection;

namespace StreamInstruments.Logging;

public static class LoggingInstaller
{
    public static void Install(IServiceCollection services)
    {
        services.AddTransient<IStreamChatLogger, StreamChatLogger>();
    }
}