using System.Text;
using System;
using System.Net.Http;
using ConductorDotnetClient.Swagger.Api;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Core;

namespace Serilog.Sinks.ConductorTaskLog
{
    /// <summary>
    ///     A sink for sending log events to Netflix Conductor
    /// </summary>
    public class ConductorTaskLogSink : ILogEventSink
    {
        private static HttpClient _logClient;
        private readonly IFormatProvider _formatProvider;
        private readonly string _logEndpoint = "tasks/{0}/log";

        /// <summary>
        ///     Initialize a new instance of ConductorTaskLogSink and explicitally specify
        ///     the url to Netflix Conductor
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="conductorUrl">The url to Netflix Conductor ending with /api/</param>
        /// <returns></returns>
        public ConductorTaskLogSink(IFormatProvider formatProvider, string conductorUrl)
        {
            _formatProvider = formatProvider;
            _logClient = new HttpClient
            {
                BaseAddress = new Uri(conductorUrl)
            };
        }

        /// <inheritdoc />
        public async void Emit(LogEvent logEvent)
        {
            HttpResponseMessage r;
            if (logEvent.Properties.TryGetValue("ConductorTaskId", out var taskId))
                r = await _logClient.PostAsync(
                    string.Format(_logEndpoint, taskId.ToString().Trim('"')),
                    new StringContent(
                        $"[{GetLevel(logEvent.Level)}] : {logEvent.RenderMessage(_formatProvider)}",
                        Encoding.UTF8,
                        "application/json"
                    )
                );
        }

        private string GetLevel(LogEventLevel level) => level switch
            {
                LogEventLevel.Fatal => "FTL",
                LogEventLevel.Error => "ERR",
                LogEventLevel.Warning => "WRN",
                LogEventLevel.Information => "INF",
                LogEventLevel.Debug => "DBG",
                LogEventLevel.Verbose => "VER",
                _ => "???"
            };
    }
}