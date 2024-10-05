# Logging Sample Code for CommandLine Programs

By default, the [Albatross.CommandLine](../../commandline/Albatross.CommandLine/) library setup serilog logging with a console sink and the default level of Warning.  
Change the default logging configuration by overriding the `ConfigureLogging` method in the `Setup` class.