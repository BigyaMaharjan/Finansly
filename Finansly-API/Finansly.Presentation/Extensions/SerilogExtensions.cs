using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;

namespace Finansly.Presentation.Extensions;

public static class SerilogExtensions
{
    private static readonly string _outputTemplate =
        "{MachineName} {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext:l}] " +
        "CorrId: {CorrelationId} TraceId: {TraceId} SpanId: {SpanId} | " +
        "Tenant: {TenantId} User: {UserId} | {Message}{NewLine}{Exception}";

    public static void ConfigureSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithSpan()
                .Enrich.WithCorrelationId()
                .WriteTo.Console(outputTemplate: _outputTemplate)
                .WriteTo.File(
                    path: "Logs/log.txt",
                    outputTemplate: _outputTemplate,
                    rollingInterval: RollingInterval.Day);
        });
    }
}
