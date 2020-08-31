using System.Threading.Tasks;
using ConductorDotnetClient.Extensions;
using ConductorDotnetClient.Interfaces;
using ConductorDotnetClient.Swagger.Api;
using Serilog.Sinks.ConductorTaskLog.Extensions;
using ConductorTask = ConductorDotnetClient.Swagger.Api.Task;
using Task = System.Threading.Tasks.Task;

namespace Serilog.Sinks.ConductorTaskLog.Sample
{
    public class SampleWorkerTask : IWorkflowTask
    {
        public string TaskType { get; } = "test_task";

        public int? Priority { get; } = 1;

        public Task<TaskResult> Execute(ConductorTask task)
        {
            using var _ = task.LogScope();
            Log.Information("Doing some work");
            return Task.FromResult(task.Completed());
        }
    }
}