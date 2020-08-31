using System;
using System.Threading;
using System.Threading.Tasks;
using ConductorDotnetClient.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Serilog.Sinks.ConductorTaskLog.Sample
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var workflowTaskCoordinator = _serviceProvider.GetRequiredService<IWorkflowTaskCoordinator>();
            foreach (var worker in _serviceProvider.GetServices<IWorkflowTask>())
                workflowTaskCoordinator.RegisterWorker(worker);

            await workflowTaskCoordinator.Start();
        }
    }
}