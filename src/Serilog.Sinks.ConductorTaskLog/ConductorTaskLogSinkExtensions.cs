using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;

namespace Serilog.Sinks.ConductorTaskLog
{
    /// <summary>
    /// ConductorTaskLog extension
    /// </summary>
    public static class ConductorTaskLogSinkExtensions
    {
        /// <summary>
        /// Add the ConductorTaskLog sink
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
        /// Add the ConductorTaskLog sink and specify a url to Netflix Conductor
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
        /// Enable the sink to use ConductorDotnetClient's configuration
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection EnableConductorTaskLog(this IServiceCollection services)
        {
            ConductorTaskLogSink.ConfigureConductorClient(services);

            return services;
        }
    }
}