# How to Create Option Sets (Mutually Exclusive Options)

## The Old Way
[CommandLineParser](https://www.nuget.org/packages/CommandLineParser) supports mutually exclusive options set by using the `SetName` property on the `OptionAttribute`.  `System.CommandLine` doesn't have this support.  But the same functionality can be achieved by using validators.  This can be done easier using the generated commands by `Albatross.CommandLine`.

In the example below, `Id` and `Name` are mutually exclusive options.  The added validator code ensure that only one of the options can be set.  Notice that the help message does not automatically reflect the custom validation code.  So manual description on the option would be required to clearly indicate the mutually exclusive requirement.
```csharp
[Verb("mutually-exclusive-command", typeof(MutuallyExclusiveCommandHandler))]
public class MutuallyExclusiveCommandOptions {
	[Option(Required = false, Description = "Describe your option requirement here")]
	public int Id { get; set; }

	[Option(Required = false, Description = "Describe your option requirement here")]
	public string Name { get; set; } = string.Empty;
}

public partial class MutuallyExclusiveCommand : IRequireInitialization {
	public void Init() {
		this.AddValidator(result => {
			var found = result.Children.Where(x => x.Symbol == this.Option_Id || x.Symbol == this.Option_Name).ToList();
			if (found.Count == 0) {
				result.ErrorMessage = "Either Id or Name is required";
			} else if (found.Count > 1) {
				result.ErrorMessage = "Id and Name are mutually exclusive";
			};
		});
	}
}
public class MutuallyExclusiveCommandHandler : BaseHandler<MutuallyExclusiveCommandOptions> {
	public MutuallyExclusiveCommandHandler(IOptions<MutuallyExclusiveCommandOptions> options, ILogger logger) : base(options, logger) {
	}
}
```
Source file is located at [MutuallyExclusiveCommandOptions.cs](../Sample.CommandLine/MutuallyExclusiveCommandOptions.cs)

## The New Way
The new way is to use sub commands.  Sub commands are natively supported by the `System.CommandLine` library.  With it, help messages will be generated correctly out of box and mutually exclusive options can be created as its own commands that may or may not share the same command handler.  

In the example below, the parent command `search` has two sub commands `id` and `name`.  They are linked to the parent command when the `Parent` property of the `Verb` attribute is set to the parent verb.  In this case, the subcommands share the same command handler `SearchCommandHandler`.  The handler can inject options from both sub commands into its constructor and decide what to do base on its values. 

Note that the parent command has the `HelpCommandHandler`.  When it is invoked without any sub command, it will display the its help messages.

```csharp
[Verb("search", typeof(HelpCommandHandler))]
public record class SearchOptions { }

[Verb("id", typeof(SearchCommandHandler), Parent = "search")]
public class SearchByIdOptions {
	public int Id { get; set; }
}
[Verb("name", typeof(SearchCommandHandler), Parent = "search")]
public class SearchByNameOptions {
	public string Name { get; set; } = string.Empty;
}
public class SearchCommandHandler : ICommandHandler {
	public SearchCommandHandler(IOptions<SearchByIdOptions>? searchByIdOptions, IOptions<SearchByNameOptions>? searchByNameOptions) {
	}

	public int Invoke(InvocationContext context) {
		// do something
		return 0;
	}

	public Task<int> InvokeAsync(InvocationContext context) {
		// do something
		return Task.FromResult(0);
	}
}
```
Source file is located at [Search.cs](../Sample.CommandLine/Search.cs)

As shown in the example above, This approach results in cleaner code and it is the better way.