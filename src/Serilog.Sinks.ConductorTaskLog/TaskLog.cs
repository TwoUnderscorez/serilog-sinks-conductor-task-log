using System;
using Serilog.Context;

namespace Serilog.Sinks.ConductorTaskLog
{
    /// <summary>
    /// A static helper class for Conductor Task Logs
    /// </summary>
    public static class TaskLog
    {
        /// <summary>
        /// A wrapper for pushing the taskId with the correct name to
        /// the LogContext
        /// </summary>
        /// <param name="taskId">The taskId logs should be associated with</param>
        /// <returns></returns>
        public static IDisposable LogScope(string taskId)
        {
            return LogContext.PushProperty("ConductorTaskId", taskId);
        }
    }
}