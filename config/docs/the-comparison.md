# The Comparison between Albatross.Config and the default IOptions<> Config setup

Overall, config through `Albatross.Config` is easier to implement than the varies `IOptions` interfaces created by the .Net library because its functionality is exposed directly through the POCO class itself.

|Albatross.Config|IOptions<> Config Setup|
|-|-|
|POCO config class can be injected as dependency directly|POCO class are injected through `IOptions<>` interface and access through `IOptions.Value` property|
|POCO class has to have a constructor with a single parameter of type `IConfiguration`|No required constructor|
|POCO class needs to derive from `ConfigBase`|No base class requirement|
| Registerd the POCO object using the Scoped lifetime to achieve the same functionality as `IOptionsSnapshot`: `AddConfig(false)` |`IOptionsSnapshot` provides a snapshot of the options at the time of the object construction |
|allow validation by DataAnnotations or custom|Validation by DataAnnotations or custom through IValidateOption<> interface|
|**POCO class can access other configuration values in the config file**|POCO class values comes from binded config values only|

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
In the case above, both secmaster and pricemaster dll have their own config requirements but they also shared common endpoints and connection strings.  This can be done with `Albatross.Config`:
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
Now these `Config` classes can be easily injected into its own libraries and we are happily on the way.  With `IOptions` interfaces, this is not so simple.  Libary authors would have to make sacrifices with the repeated config values as show below: 
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