using System;
using System.Net.Http;
using ConductorDotnetClient.Swagger.Api;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.ConductorTaskLog
{
    /// <summary>
    ///     A sink for sending log events to Netflix Conductor
    /// </summary>
    public class ConductorTaskLogSink : ILogEventSink
    {
        private static IConductorRestClient _conductorClient;
        private static IServiceCollection _serviceCollection;
        private readonly IFormatProvider _formatProvider;

        /// <summary>
        ///     Initialize a new instance of ConductorTaskLogSink
        /// </summary>
        /// <param name="formatProvider"></param>
        public ConductorTaskLogSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        /// <summary>
        ///     Initialize a new instance of ConductorTaskLogSink and explicitally specify
        ///     the url to Netflix Conductor
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="conductorUrl">The url to Netflix Conductor ending with /api/</param>
        /// <returns></returns>
        public ConductorTaskLogSink(IFormatProvider formatProvider, string conductorUrl) : this(formatProvider)
        {
            _conductorClient = new CustomConductorRestClient(
                new HttpClient
                {
                    BaseAddress = new Uri(conductorUrl)
                }
            );
        }

        /// <inheritdoc />
        public async void Emit(LogEvent logEvent)
        {
            if ((_conductorClient ??=
                _serviceCollection?.BuildServiceProvider()?.GetService<IConductorRestClient>()) is null)
                return;
            var level = logEvent.Level switch
            {
                LogEventLevel.Fatal => "FTL",
                LogEventLevel.Error => "ERR",
                LogEventLevel.Warning => "WRN",
                LogEventLevel.Information => "INF",
                LogEventLevel.Debug => "DBG",
                LogEventLevel.Verbose => "VER",
                _ => throw new ArgumentOutOfRangeException($"Invalid {nameof(LogEventLevel)}")
            };
            if (logEvent.Properties.TryGetValue("ConductorTaskId", out var taskId))
                await _conductorClient.LogAsync(
                    taskId.ToString().Trim('"'),
                    $"[{level}] : {logEvent.RenderMessage(_formatProvider)}"
                ).ConfigureAwait(false);
        }

        /// <summary>
        ///     Configure the ConductorTaskLogSink to use the url provided when
        ///     configuring ConductorDotnetClient
        /// </summary>
        /// <param name="services">A service collection with ConductorDotnetClient configured eventually</param>
        public static void ConfigureConductorClient(IServiceCollection services)
        {
            _serviceCollection = services;
        }
    }
}