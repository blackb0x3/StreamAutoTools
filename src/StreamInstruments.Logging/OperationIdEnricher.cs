using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace StreamInstruments.Logging;

public class OperationIdEnricher : ILogEventEnricher
{
    private const string OperationIdPropertyName = "OperationId";
    private const string OperationParentIdPropertyName = "ParentId";
    
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;

        if (activity is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(activity.Id))
        {
            return;
        }

        logEvent.AddPropertyIfAbsent(new LogEventProperty(OperationIdPropertyName, new ScalarValue(activity.Id.Replace("-", string.Empty))));
        logEvent.AddPropertyIfAbsent(new LogEventProperty(OperationParentIdPropertyName, new ScalarValue(activity.ParentId)));
    }
}