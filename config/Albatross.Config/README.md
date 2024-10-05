# Albatross.Config
Simplfied configuration setup for your .Net applications.

## Features
* Use Strong typed POCO configuration class without using the IOptions<> interface.
* Built-In [ProgramSetting](./ProgramSetting.cs) and [EnvironmentSetting](./EnvironmentSetting.cs) config class.
* Built-In endpoints configuration key similar to the connectionStrings key provided by default.
* Validate configuration data using data annotation attributes in the System.DataAnnotation namespace or implement your own validation.
* Custom implementation of IHostEnvironment to save us from changing prod data by accident.  See [HELP!  My developers are also DBAs](../docs/hosting-env.md).

## Related Articles
* [The Comparison between Albatross.Config and the default IOptions<> Setup](../docs/the-comparison.md)
* [.Net Hosting Environments](../docs/hosting-env.md)
* [Use of Environment Variables and Command Line Parameters](../docs/hosting-env.md)

## Quick Start
`Albatross.Config` requires the .Net configuration to be setup correctly and an instance of `IConfiguration` can be injected through DI.  A config POCO class can be created using base class `Albatross.Config.ConfigBase`.  It's required to have a constructor with a single parameter of type `Microsoft.Extensions.Configuration.IConfiguration`.

Here is an example of appsettings.json file and the matching config POCO file.
```json
{
	"connectionStrings" : {
		"azure-db" : "..."
	},
	"endpoints": {
		"ms-graph" : "..."
	},
	"my-config" : {
		"configData" : "..."
	}
}
```
```csharp
public class MyConfig : ConfigBase {
	public MyConfig(IConfiguration configuration) {
		this.MsGraphicEndPoint = configuration.GetRequiredEndPoint("ms-graph");
		this.MyConnectionString = configuration.GetRequiredConnectionString("azure-db");
	}
	public override Key => "my-config";
	[Required]
	public string ConfigData { get; }
	public string MsGraphicEndPoint { get; }
	public string MyConnectionString { get; }
	public override void Validate() {
		base.Validate();
	}
}
```
If the Key of the config class is defined, the json values of the key in the `appsettings.json` file will be binded to the config class.  The `Validate` method of the class will be invoked after the creation of its instance.  An exception will be thrown if there are any validation errors.

Once the config class is registered using DI, it can be injected directly to classes as a dependency.  The `AddConfig` method by default registers the config class as a Singleton.  `AddConfig(false)` will register the class with a scoped lifetime.  The scoped lifetime is useful in the rare case of config data change.
```csharp
	public static IServiceCollection RegisterMyServices(this IServiceCollection services) {
		services.AddConfig<MyConfig>();
		return services;
	}
```