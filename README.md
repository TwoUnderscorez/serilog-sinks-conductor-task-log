# Serilog.Sinks.ConductorTaskLog
A serilog sink that sends task logs to [Netflix Conductor](https://github.com/Netflix/conductor).



### Getting started
#### With [conductor-dotnet-client](https://github.com/courosh12/conductor-dotnet-client)
1. Add the sink to your logger configuration (typically in `Program.cs`)
    ```csharp
    Log.Logger = new LoggerConfiguration()
        ...
        .WriteTo.ConductorTaskLog() // <-- Add the sink
        .Enrich.FromLogContext() // <-- Also add this enricher
        .CreateLogger();
    ```

2. Enable the task log (typically in `Program.cs` or `Startup.cs`
    ```csharp
    services.EnableConductorTaskLog()
    ```

   The sink will get the connection string to the Conductor from conductor-dotnet-client's configuration.
3. Add this line at the start of your `Execute` method to let the sink know the taskId.
    ```csharp
    using var _ = (LogContext.PushProperty("ConductorTaskId", task.TaskId));
    ```

#### With something else
1. Add the sink to your logger configuration (typically in `Program.cs`) and specify a URI to the conductor.
    ```csharp
    Log.Logger = new LoggerConfiguration()
        ...
        .WriteTo.ConductorTaskLog("http://localhost:8080/api") // <-- Add the sink
        .CreateLogger();
    ```

2. Add this line at the start of your any method to log all events from that method
    ```csharp
    using var _ = (LogContext.PushProperty("ConductorTaskId", "TaskId"));
    ```
    or like so to only log a few lines to the conductor
    ```csharp
    Log.Information("Not logging to Netflix Conductor");
    using (LogContext.PushProperty("ConductorTaskId", "TaskId"))
    {
        Log.Information("Log sent to Netflix Conductor");
    }
    ```