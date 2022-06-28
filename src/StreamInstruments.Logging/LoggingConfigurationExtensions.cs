using Serilog;
using Serilog.Configuration;

namespace StreamInstruments.Logging;

public static class LoggingConfigurationExtensions
{
    public static LoggerConfiguration WithOperationId(this LoggerEnrichmentConfiguration enrich)
    {
        if (enrich == null)
            throw new ArgumentNullException(nameof(enrich));

        return enrich.With<OperationIdEnricher>();
    }
}