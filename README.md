# <img src="https://raw.githubusercontent.com/TwoUnderscorez/serilog-sinks-conductor-task-log/init_sln/images/icon.png" width="30" height="30" /> Serilog.Sinks.ConductorTaskLog [![Build Status](https://planq.visualstudio.com/Serilog.Sinks.ConductorTaskLog/_apis/build/status/TwoUnderscorez.serilog-sinks-conductor-task-log?branchName=master)](https://planq.visualstudio.com/Serilog.Sinks.ConductorTaskLog/_build/latest?definitionId=1&branchName=master) ![Deployment status](https://planq.vsrm.visualstudio.com/_apis/public/Release/badge/1baf4033-9413-4342-a1c7-d3bf4f65b6af/1/1) [![NuGet Version](https://img.shields.io/nuget/v/Serilog.Sinks.ConductorTaskLog.svg?style=flat)](https://www.nuget.org/packages/Serilog.Sinks.ConductorTaskLog/)

A serilog sink that sends task logs to [Netflix Conductor](https://github.com/Netflix/conductor).
Don't use this sink as your only sink.

## Showcase

![Showcase](images/example.png)

## Getting started

### Configuring

#### In code

Add the sink to your logger configuration (typically in `Program.cs`)

```csharp
Log.Logger = new LoggerConfiguration()
      ...
      .WriteTo.ConductorTaskLog("http://conductor:8080/api/") // <-- Add the sink
      .Enrich.FromLogContext() // <-- Also add this enricher
      .CreateLogger();
```

#### In appsettings.json

```json
{
   "Serilog": {
      "WriteTo": [
         {
            "Name": "ConductorTaskLog",
            "Args": {
               "conductorUrl": "http://conductor:8080/api/"
            }
         }
      ],
      "Enrich": [
         "FromLogContext"
      ]
   }
}
```

### Using

#### With [conductor-dotnet-client](https://github.com/courosh12/conductor-dotnet-client)

Add the using
```csharp
using Serilog.Sinks.ConductorTaskLog.Extensions;
```
The add this line at the start of your `Execute` method to let the sink know the taskId.
```csharp
using var _ = task.LogScope();
```

#### With something else

Add this line at the start of any method to log all events from that method
```csharp
using Serilog.Sinks.ConductorTaskLog;

using var _ = TaskLog.LogScope("taskId");
```
or like so to only log a few lines to the conductor
```csharp
Log.Information("Not logging to Netflix Conductor");
using (TaskLog.LogScope("taskId"))
{
      Log.Information("Log sent to Netflix Conductor");
}
```
