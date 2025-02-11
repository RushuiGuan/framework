# Albatross.Logging
Quick logging setup for your .Net application using Serilog.

## Features
- [ErrorMessage enricher](../docs/error-msg-enricher.md)
- [Shortened Logger Name](../docs/shortened-logger-name.md)
- [Enhanced SlackSink](../docs/enhanced-slacksink.md)

## Quick Start
`Albatross.Logging` are integrated to the Albatross hosting projects below.
* [Albatross.Hosting](../../hosting/Albatross.Hosting/)
* [Albatross.CommandLine](../../commandline/Albatross.CommandLine/)
* [Albatross.Hosting.Excel](../../excel/Albatross.Hosting.Excel/)
* [Albatross.ReqnrollPlugin](../../testing/Albatross.ReqnrollPlugin/)

To setup `Albatross.Logging` on your own, see the code below:
```csharp
var logger = new SetupSerilog().Configure(ConfigureLogging).Create();
```
However, the actual serilog logger is usually not used in the application.  It is preferred to use [ILogger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) or [ILogger`1](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger-1?view=net-8.0) interface so that the bulk of the application is not couple to the serilog library.  To setup the connection, use the code below:
```csharp
var builder = Host.CreateDefaultBuilder();
hostBuilder.UseSerilog(logger);
```