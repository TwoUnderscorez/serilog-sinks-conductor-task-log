using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;
using ConductorTask = ConductorDotnetClient.Swagger.Api.Task;

namespace Serilog.Sinks.ConductorTaskLog.Extensions
{
    /// <summary>
    ///     ConductorTaskLog extension
    /// </summary>
    public static class ConductorTaskLogSinkExtensions
    {
        /// <summary>
        ///     Add the ConductorTaskLog sink
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static LoggerConfiguration ConductorTaskLog(
            this LoggerSinkConfiguration loggerConfiguration,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(
                new ConductorTaskLogSink(formatProvider));
        }

        /// <summary>
        ///     Add the ConductorTaskLog sink and specify a url to Netflix Conductor
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="conductorUrl">A url to Netflix Conductor ending with /api/</param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static LoggerConfiguration ConductorTaskLog(
            this LoggerSinkConfiguration loggerConfiguration, string conductorUrl,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(
                new ConductorTaskLogSink(formatProvider, conductorUrl));
        }

        /// <summary>
        ///     Enable the sink to use ConductorDotnetClient's configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection EnableConductorTaskLog(this IServiceCollection services)
        {
            ConductorTaskLogSink.ConfigureConductorClient(services);

            return services;
        }

        /// <summary>
        ///     A wrapper for <see cref="TaskLog.LogScope" />
        /// </summary>
        /// <param name="task">this</param>
        /// <returns></returns>
        public static IDisposable LogScope(this ConductorTask task)
        {
            return TaskLog.LogScope(task.TaskId);
        }
    }
}