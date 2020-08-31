using System;
using Serilog.Context;

namespace Serilog.Sinks.ConductorTaskLog
{
    /// <summary>
    ///     A static helper class for Conductor Task Logs
    /// </summary>
    public static class TaskLog
    {
        /// <summary>
        ///     A wrapper for pushing the <paramref name="taskId" /> with
        ///     the correct name to the <see cref="LogContext" />
        ///     The name of the property used by this function is <c>ConductorTaskId</c>
        ///     so be careful as to not accidentally use it.
        /// </summary>
        /// <param name="taskId">The taskId logs should be associated with</param>
        /// <returns>
        ///     A disposable that should be disposed of when no more logs should
        ///     be logged for the specified <paramref name="taskId" />
        /// </returns>
        public static IDisposable LogScope(string taskId)
        {
            return LogContext.PushProperty("ConductorTaskId", taskId);
        }
    }
}