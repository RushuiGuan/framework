# Create a CommandLine Application
[Albatross.CommandLine](xref:Albatross.CommandLine) assembly can be used to create a console program with DI and logging support.  It uses [System.CommandLine](https://www.nuget.org/packages/System.CommandLine) to parse console input parameters.  It provides simple bootstrapping of commands with builtIn DI support using [.NET Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection).  It also uses [Serilog](https://serilog.net/) as its logging library.

Sample project can be found here: [Sample.CommandLine](xref:Sample.CommandLine).

Follow the steps below to create a new commandline utility.
1. Create a new .Net Console program that targets version 6, 7 or 8.
1. Reference package [Albatross.CommandLine](https://www.nuget.org/packages/Albatross.CommandLine).
1. Reference package [Albatross.CommandLine.CodeGen](https://www.nuget.org/packages/Albatross.CommandLine.CodeGen) and mark it as PrivateAssets
	```xml
	<PackageReference Include="Albatross.CommandLine.CodeGen" Version="7.4.0">
		<PrivateAssets>All</PrivateAssets>
	</PackageReference>
	```
1. Create a class called `MySetup.cs` that inherits [Albatross.CommandLine.Setup](xref:Albatross.CommandLine.Setup).
	Overwrite the `RegisterServices` method with the code below:
	```csharp
	public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
		base.RegisterServices(context, configuration, envSetting, services);
		// this line registers your commands automatically.  RegisterCommands method is created by codegen.
		services.RegisterCommands();
		// register your own dependencies here
		services.AddSingleton<IMyInterfacce, MyImplementation>();
		// register command specific registration using a switch statement
		switch(context.ParsedCommandName()) {
			case "command1":
				services.AddSingleton<Command1Dependency>();
			break;
			case "command2":
				services.AddSingleton<Command2Dependency>();
			break;
	}
	```
1. Replace the Program.cs file with the code below.
	```csharp
	namespace MyCommandLine {
		class Program {
			static async Task<int> Main(string[] args) {
				var setup = new MySetup().AddCommands();
				var parser = setup.CommandBuilder.Build();
				return await parser.InvokeAsync(args);
			}
		}
	}
	```
1. To create a new command, create a new class called `MyCommandOptions`.  `MyCommandOptions` class contains that verb of the command as well as any command parameters (options).  Note that by convension, the options class should be postfixed with the word "Options".  The attribute class [VerbAttribute](xref:Albatross.CommandLine.VerbAttribute) is used to specify the verb of the command as well as the implementation class of the command.
	```csharp
	[Verb("my-command", typeof(MyCommandHandler))]
	public record class MyCommandOptions {
		[Option(Alias = ["n"])]
		public int Name {get; set;} = string.Empty;
	}
	public class MyCommandHandler : ICommandHandler {
		MyCommandOptions options;
		public MyCommandHandler(IOptions<MyCommandOptions> options) {
			this.options = options.Value;
		}
	}
	```
