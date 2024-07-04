using Amazon.Runtime;
using MapSolution.Configurations;
using Microsoft.AspNetCore.HttpOverrides;
using MongoDB.Driver;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace MapSolution;

public static class ConfigureServices
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(o => o.AddPolicy("Cors", builder =>
        {
            builder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }));

        return services;
    }
    public static IServiceCollection AddRoutingInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddRouting(o =>
        {
            o.LowercaseUrls = true;
            o.LowercaseQueryStrings = true;
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            // cfg.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
        });

        services.AddHealthChecks();

        return services;
    }
    public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        MongoOptions mongoOptions = new();
        configuration.GetSection(MongoOptions.Section).Bind(mongoOptions);

        mongoOptions.Validate();

        services.AddSingleton(mongoOptions);

        int port = int.Parse(mongoOptions.Port);
        MongoServerAddress serverAddress = new MongoServerAddress(mongoOptions.Host, port);

        MongoUrlBuilder connectionStringBuilder = new MongoUrlBuilder();

        if (mongoOptions.Username != String.Empty)
        {
            connectionStringBuilder.Username = mongoOptions.Username;
        }

        if (mongoOptions.Password != String.Empty)
            connectionStringBuilder.Password = mongoOptions.Password;

        connectionStringBuilder.Server = serverAddress;

        string mongoConnectionString = connectionStringBuilder.ToMongoUrl().ToString();

        services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
        
        return services;
    }
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