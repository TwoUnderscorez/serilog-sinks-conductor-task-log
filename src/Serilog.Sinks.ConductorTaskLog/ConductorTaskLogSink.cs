using Serilog.Core;
using Serilog.Events;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using ConductorDotnetClient.Swagger.Api;
using Microsoft.Extensions.Logging;

namespace Serilog.Sinks.ConductorTaskLog
{
    public class ConductorTaskLogSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        private static IConductorRestClient _conductorClient;
        private static IServiceCollection _serviceCollection;

        public ConductorTaskLogSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public ConductorTaskLogSink(IFormatProvider formatProvider, string conductorUrl) : this(formatProvider)
        {
            _conductorClient = new CustomConductorRestClient(
                new HttpClient
                {
                    BaseAddress = new Uri(conductorUrl)
                }
            );
        }

        public async void Emit(LogEvent logEvent)
        {
            if ((_conductorClient ??= _serviceCollection?.BuildServiceProvider()?.GetService<IConductorRestClient>()) is null)
                return;
            var level = logEvent.Level switch
            {
                LogEventLevel.Fatal => "FTL",
                LogEventLevel.Error => "ERR",
                LogEventLevel.Warning => "WRN",
                LogEventLevel.Information => "INF",
                LogEventLevel.Debug => "DBG",
                LogEventLevel.Verbose => "VER"
            };
            if (logEvent.Properties.TryGetValue("ConductorTaskId", out var taskId))
                await _conductorClient.LogAsync(
                    taskId.ToString().Trim('"'),
                    $"[{level}] : {logEvent.RenderMessage(_formatProvider)}"
                ).ConfigureAwait(false);
        }

        public static void ConfigureConductorClient(IServiceCollection services)
        {
            _serviceCollection = services;
        }
    }
}
