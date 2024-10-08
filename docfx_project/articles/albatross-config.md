## Albatross.Config assembly

Albatross framework has a configuration framework to simplify the setup of custom configuration values.  Its namespace is [Albatross.Config]
(xref:Albatross.Config)

### Custom configuration:
1. Create a config class MySetting with its base class as [Albatross.Config.ConfigBase](xref:Albatross.Config.ConfigBase).
	```c#
	public class MySettings : ConfigBase {
		public override string Key => "my-settings";
		public MySettings(IConfiguration configuration): base(configuration) {
			this.ConnectionString = configuration.GetRequiredConnectionString("my-db");
			this.GraphEndPoint = configuration.GetRequiredEndPoint("graph");
		}

		public int DbCommandTimeOut { get; set; }
		public string ConnectionString { get; set; }
		public string GraphEndPoint { get; set; }
	}
	```
1. Create a new section called `my-settings` in the appsettings.json config file and add the configuration value within the section.  The values in `MySettings` class will be deserialized from the json section and populated automatically when the class instance is created.  In its constructor, it has access to the [IConfiguration](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfiguration?view=dotnet-plat-ext-7.0) class and it can be used to get value from other shared sections such as ConnectionStrings and EndPoints.  Two `IConfiguration` extension methods named `GetRequiredConnectionString` and `GetRequiredEndPoint` are very handy in getting shared connection strings and endpoints.
	```json
	{
		"connectionString": [
			"my-db": "Server=.;Database=my-db;Trusted_Connection=true;Encrypt=false",
			"his-db": "Server=.;Database=his-db;Trusted_Connection=true;Encrypt=false"
		],
		"endpoints":[
			"graph": "http://dev-server/graph/api",
			"directory": "http://dev-server/directory/api"
		],
		"my-settings" : {
			"dbCommandTimeOut" : 60
		}
	}
	```
1. Register the configuration class by using the `AddConfig` extension method located in [Extension](xref:Albatross.Config.Extension) class.  Once registered, the config class `MySettings` can be injected to any class as a dependency as shown in the example class `MyService` below.  By default, `AddConfig` will register config class as a singleton class.  Change the value of the parameter `singleton` to false and the class will be registered using scoped lifetime.  It could be useful if there is a requirement for the change of configuration value to be reflected without restarting the application.
	```c#
	public static IServiceCollection AddMySettings(this IServiceCollection services) {
		services.AddConfig<MySettings>();
		return services;
	}
	public class MyService {
		private readonly MySettings mySettings;
		public class MyServiceClass(MySettings mySettings) {
			this.mySettings = mySettings;
		}
	}
	```
### [ProgramSetting](xref:Albatross.Config.ProgramSetting) Config Class
`ProgramSetting` is a built in config class with the section name of `app` provided by the [webapi](webapi.md), [service](service.md) and [utility](utility.md) hosts.  The `Name` and `Group` properties are used to identify and organize the said application.  This is a useful functionality in a micro service environment when large numbers of applications have to be managed by an organization.  Another important property is `ServiceManager`.  It can have 2 possible values:
- windows
- systemd

`windows` is for creating a service in windows environment and `systemd` is for linux environment.  Please find details in the [how to create services using the framework](service.md) article.

### [EnvironmentSetting](xref:Albatross.Config.EnvironmentSetting) Config Class
The `EnvironmentSetting` class provides the same functionality as the [IHostEnvironment.EnvironmentName](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment.environmentname?view=dotnet-plat-ext-7.0#microsoft-extensions-hosting-ihostenvironment-environmentname) property from Microsoft.  But it has a different default value when the environmental variable is missing.  When the `ASPNETCORE_ENVIRONMENT` or `DOTNET_ENVIRONMENT` variable is missing, the [IHostEnvironment.EnvironmentName](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment.environmentname?view=dotnet-plat-ext-7.0#microsoft-extensions-hosting-ihostenvironment-environmentname) property will return the text string of `PRODUCTION`.  The reason from Microsoft is that developers will never have credentials for the production environment therefore it is the "safest" default environmental setting.  That might be the truth for Microsoft developers.  But it could be a dangerous thing to do for developers of small and medium enterprises.  To avoid the potentially disastrous problems of writing bad data to the prod enviornment, `EnvironmentSetting` was created and it will simply return an empty string when the targeted environment variable is missing.

`EnvironmentSetting` is provided by the [webapi](webapi.md), [service](service.md) and [utility](utility.md) hosts.  For [webapi](webapi.md) and [service](service.md) hosts, it sets the Value property using environment variable `ASPNETCORE_ENVIRONMENT`.  For [utility](utility.md) hosts, it uses `DOTNET_ENVIRONMENT`.  It has a IsProd property that returns true if the config value equals `production` (case insensitive comparison).  As mentioned in the chapter above, the Value property will be an empty string when the environmental variable is missing.
