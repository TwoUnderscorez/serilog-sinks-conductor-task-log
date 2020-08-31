using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConductorDotnetClient;
using ConductorDotnetClient.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.ConductorTaskLog.Extensions;
using Serilog;
using Serilog.Events;

namespace Serilog.Sinks.ConductorTaskLog.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.ConductorTaskLog()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting task runner");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Task runner terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddHostedService<Worker>()
                        .AddConductorWorkflowTask<SampleWorkerTask>()
                        .AddConductorWorker(new ConductorClientSettings
                        {
                            ConcurrentWorkers = 1,
                            IntervalStrategy = ConductorClientSettings.IntervalStrategyType.Linear,
                            MaxSleepInterval = 15_000,
                            SleepInterval = 1_000,
                            ServerUrl = new Uri("http://localhost:8080/api/")
                        })
                        .EnableConductorTaskLog();
                });
    }
}
