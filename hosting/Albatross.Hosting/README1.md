# Albatross.Hosting assembly is a bootstrapping assembly that can be used to create a aspnetcore webapp or a service application.

## Create a new webapp
* Create a new console application
* Open the project file and change the Project Sdk to `Microsoft.NET.Sdk.Web`
* Add a reference to the Albatross.Hosting assembly
* Create an `appsettings.json`, `hostsettings.json` and `serilog.json` file in the project root
* Set both files to `Copy to Output Directory` = `Copy if newer`
* Edit the `hostsettings.json` file and add the following code
	```json
	{
		"urls": "http://localhost:5000"
	}
	```
* Add the following code to the Main method of the `Program.cs` file
* Create a new class `MyStartup.cs` that inherits from `Albatross.Hosting.Startup`
* Add the following code to the `Startup.cs` file
	```csharp
	public class MyStartup : Albatross.Hosting.Startup {
		public MyStartup(IConfiguration configuration) : base(configuration) {
		}
	}
	```
* Replace the Main method of the `Program.cs` file with the code below
	```csharp
	public static Task Main(string[] args) {
		return new Setup(args)
			.ConfigureWebHost<MyStartup>()
			.RunAsync();
	}
	```
```csharp
* These steps will create a new webapp that listens on `http://localhost:5000`
