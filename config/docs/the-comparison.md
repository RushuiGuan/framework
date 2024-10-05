# The Comparison between Albatross.Config and the default IOptions<> Config setup

Overall, config through `Albatross.Config` is easier to implement than the varies `IOptions` interfaces created by the .Net library because its functionality is exposed directly through the configuration class itself.

|Albatross.Config|IOptions<> Config Setup|
|-|-|
|Config class can be injected as dependency directly|POCO class are injected through `IOptions<>` interface and access through `IOptions.Value` property|
|Config class requires a constructor with a single parameter of type `IConfiguration`|No required constructor|
|Config class requires a base class of `ConfigBase`|No base class requirement|
| Registerd the config class using the Scoped lifetime `AddConfig(false)` to achieve the same functionality as `IOptionsSnapshot` |`IOptionsSnapshot` provides a snapshot of the options at the time of the object construction |
|allow validation by DataAnnotations or create custom validations by overriding the Validate method|Validation by DataAnnotations or create custom validations through IValidateOption<> interface|
|**Config class can access other configuration values in the config file**|POCO class values comes from binded config values only|

## A Not-Very-Obvious Advantage of `Albatross.Config`
Configurations can be part of a library that would be consumed by different applications.  Libraries would have its own config section in the configuration file.  What happens when two libraries need to share the same config value?  Here is an example:
```json
{
	"endpoints" : {
		"slack" : "my-slack-endpoint"
	},
	"connectionStrings" : {
		"db" : "my-db"
	},
	"secmaster" : {
		"source" : "BB"
	},
	"pricemaster" : {
		"rootPath" : "\\\\data"
	}
}
```
In the case above, both secmaster and pricemaster library have their own config requirements but they also shared common endpoints and connection strings.  This can be done with `Albatross.Config`:
```csharp
public class SecMasterConfig : ConfigBase {
	public SecMasterConfig(IConfiguration configuration) {
		this.ConnectionString = configuration.GetRequiredConnectionString("db");
		this.SlackEndPoint = configuration.GetRequiredConnectionString("slack");
	}
	public override string Key => "secmaster";
	public string ConnectionString { get; }
	public string SlackEndPoint { get; }
	[Required]
	public string Source { get; set; }
}
public class PriceMasterConfig : ConfigBase {
	public PriceMasterConfig(IConfiguration configuration) {
		this.ConnectionString = configuration.GetRequiredConnectionString("db");
		this.SlackEndPoint = configuration.GetRequiredConnectionString("slack");
	}
	public override string Key => "pricemaster";
	public string ConnectionString { get; }
	public string SlackEndPoint { get; }
	[Required]
	public string RootPath { get; set; }
}
```
Now these config classes can be easily injected into its own libraries and we are happily on the way.  With `IOptions` interfaces, this is not so simple.  Libary authors would have to make sacrifices with the repeated config values as show below: 
```json
{
	"secmaster" : {
		"slack" : "my-slack-endpoint",
		"db" : "my-db",
		"source" : "BB"
	},
	"pricemaster" :{
		"slack" : "my-slack-endpoint",
		"db" : "my-db",
		"rootPath" : "\\\\data"
	}
}
```