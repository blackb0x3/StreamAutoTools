using Serilog;

namespace StreamInstruments.Logging;

public static class LoggerGenerator
{
    public static void SetupLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .CreateLogger();
    }
}