## Logging Setup
Logging is managed by [Albatross.Logging](https://rushuiguan.github.io/framework/api/Albatross.Logging.html) assembly. `Albatross.Logging` uses [Serilog](https://serilog.net/) as its logging implementation and consumers are expected to use [Microsoft.Extensions.Logging.ILogger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-7.0) to do the actual logging.  So `Albatross.Logging`'s only job is to setup and integrate Serilog to the host program.

Serilog logging is automatically setup for [webapi](webapi.md), [service](service.md) and [utility](utility.md) hosts.  Upon startup, [ILogger](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-7.0) interface is registered as a singleton.  Consumers can also inject an [ILogger&lt;TCategoryName&gt;](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger-1?view=dotnet-plat-ext-7.0) instance to write logs with source context.

For `webapi` and `service` hosts, logging can be setup using the `serilog.json` config file at the root of the project.  Environment specific logging configs can be setup using `serilog.{environment}.json` file.  Remember to set the file property `Copy to Output Directory` = `Copy if newer` for these files.  Here is the serilog [documentation](https://github.com/serilog/serilog-settings-configuration) on how to setup logging using json files.  Please see the sample config file below
```json
{
    "Serilog": {
        "MinimumLevel": {
            "Default": "Debug",
            "Override": {
                "System": "Information",
                "Microsoft": "Information"
            }
        },
        "WriteTo": [
            {
                "Name": "Console",
                "Args": {
                    "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ssz} [{Level:w3}] {SourceContext} {Message:lj}{NewLine}{Exception}"
                }
            },
            {
                "Name": "File",
                "Args": {
                    "path": "log\\log.txt",
                    "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ssz} [{Level:w3}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                    "rollingInterval": "Day"
                }
            }
        ],
        "Enrich": [ "FromLogContext" ]
    }
}
```

For `utility` hosts, the default logging configuration is configured using a colored console sink.  To change, overwrite the `ConfigureLogging` method in the [UtilityBase](https://rushuiguan.github.io/framework/api/Albatross.Hosting.Utility.UtilityBase-1.html) class.  Here is an example of changing the logging config in the utility application:
```c#
protected override void ConfigureLogging(LoggerConfiguration cfg) {
	cfg.MinimumLevel.Information()
		.WriteTo
		.Console(outputTemplate: DefaultOutputTemplate)
		.Enrich.FromLogContext();
}
```

