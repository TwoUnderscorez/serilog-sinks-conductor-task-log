using System;
using ConductorDotnetClient;
using ConductorDotnetClient.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog.Sinks.ConductorTaskLog.Extensions;

namespace Serilog.Sinks.ConductorTaskLog.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.ConductorTaskLog("http://localhost:8080/api/")
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
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
                        });
                });
        }
    }
}