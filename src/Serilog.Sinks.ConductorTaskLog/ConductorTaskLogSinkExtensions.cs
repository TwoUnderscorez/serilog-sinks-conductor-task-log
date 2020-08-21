using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;

namespace Serilog.Sinks.ConductorTaskLog
{
    public static class ConductorTaskLogSinkExtensions
    {
        public static LoggerConfiguration ConductorTaskLog(
                this LoggerSinkConfiguration loggerConfiguration,
                IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(
                new ConductorTaskLogSink(formatProvider));
        }

        public static LoggerConfiguration ConductorTaskLog(
            this LoggerSinkConfiguration loggerConfiguration, string conductorUrl,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(
                new ConductorTaskLogSink(formatProvider, conductorUrl));
        }

        public static IServiceCollection EnableConductorTaskLog(this IServiceCollection services)
        {
            ConductorTaskLogSink.ConfigureConductorClient(services);

            return services;
        }
    }
}