# Albatross.Hosting 
A bootstrapping assembly for web api and service applications.

## Features
* Logging integration with Serilog
* Simplified configuration management using [Albatross.Config](../config/Albatross.Config/README.md) library.
* Built-in http request logging middleware
* Built-in plain text input formatter
* Unified response error format
* Built-in SPA project hosting support

## Quick Start
### Create a new WebApi application
* Create a new console application that targets net8.0
* Open the project file and change the Project Sdk to `Microsoft.NET.Sdk.Web`
* Add a reference to the Albatross.Hosting assembly
* Create an `appsettings.json`, `hostsettings.json` and `serilog.json` file in the project root
* Set all three files to `Copy to Output Directory` = `Copy if newer`
* Edit the `hostsettings.json` file and add the following code
	```json
	{
		"urls": "http://localhost:5000"
	}
	```
* Update the Main method of the`Program.cs` file with code below.
	```csharp
	public static Task Main(string[] args) {
		return new Setup(args)
			.ConfigureWebHost<Startup>()
			.RunAsync();
	}
	```
* These steps will create a new webapp that listens on `http://localhost:5000`

## Related Articles

