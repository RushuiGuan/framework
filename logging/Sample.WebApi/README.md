# Logging Sample Code for WebApi and Service Programs

By default, the [Albatross.Hosting](../../hosting/Albatross.Hosting/) library setup serilog logging with a serilog configuration file.  The documentation for figuring 
serilog by settings file can be found [here](https://github.com/serilog/serilog-settings-configuration).  The default config can be changed by overriding the `ConfigureLogging` method in the `Setup` class.