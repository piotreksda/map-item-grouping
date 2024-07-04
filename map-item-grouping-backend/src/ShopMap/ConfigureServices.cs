using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace MapSolution;

public static class ConfigureServices
{
    public static WebApplicationBuilder UseOpenTelemetry(this WebApplicationBuilder builder)
    {
        var serviceName = "ShopCrud";
        var serviceVersion = "1.0.0";
        
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion);
        
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
                {
                    metrics.SetResourceBuilder(resourceBuilder);
                    metrics.AddMeter("Microsoft.AspNetCore.Hosting");
                    metrics.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
                    metrics.AddMeter("System.Net.Http");
                    metrics.AddMeter("NServiceBus.Core");
                    metrics.AddOtlpExporter((exporterOptions, metricReaderOptions) =>
                    {
                        metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 5000;
                    });
                }
            )
            .WithTracing(traces =>
            {
                traces.AddSource("NServiceBus.Core");
            });
        
        return builder;
    }
}