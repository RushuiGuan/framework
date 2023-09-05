# New Utility Application
[Albatross.Hosting.Utility](https://rushuiguan.github.io/framework/api/Albatross.Hosting.Utility.html) assembly can be used to create a console program with DI and logging support.  Other functionalities such as messaging can be plugged in via the DI framework.  

Albatross.Hosting.Utility uses [CommandLineParser](https://www.nuget.org/packages/CommandLineParser) to parse console input parameters.  Here is the [CommandLineParser documentation](https://github.com/commandlineparser/commandline/wiki).  Each utility program can support multiple commands (verbs).  Follow the steps below to create a new utility.
1. Create a new .Net 7 Console program.
1. Reference package [Albatross.Hosting.Utility](https://www.nuget.org/packages/Albatross.Hosting.Utility).
1. Replace the Program.cs file with the code below.  Remember to change the namespace.
	```csharp
	using Albatross.Hosting.Utility;
	using CommandLine;
	using System.Threading.Tasks;
	namespace MyNameSpace.Utility {
		class Program {
			static Task<int> Main(string[] args) {
				return Parser.Default.Run(args, typeof(Program).Assembly);
			}
		}
	}
	```
1. Create a command class `RunMyCommand` and its matching Option class `RunMyCommandOption` below.  Command class contains the implementation class.  Option class contains the verb and options that will be used for command line parameter parsing.  The comments on each method of teh `RunMyCommand` class indicates the life cycle of the utility.  **Note that the `RunUtility` method is by convention and can be defined with any injectable parameters.  However, its name and return parameters should remain unchanged.**
	```csharp
	[Verb("run-my-command")]
	public class RunMyCommandOption {
		[Option('n', "name", Required =	true)]
		public string Name { get; set; } = String.Empty;
	}
	public class RunMyCommand: UtilityBase<RunMyCommandOption> {
		// step 0, constructor
		public RunMyCommand(RunMyCommandOption option) : base(option) {
		}
		// step 1, di registration
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting environmentSetting, IServiceCollection services) {
			base.RegisterServices(configuration, environmentSetting, services);
		}
		// step 2, initialization
		public override void Init(IConfiguration configuration, IServiceProvider provider) {
			base.Init(configuration, provider);
		}
		// step 3, command logic
		public async Task<int> RunUtility(SomeProxyService svc) {
			var data = await svc.GetData(Options.Name);
			Console.WriteLine(data);
			return 0;
		}
		// step 4, clean up
		public override void Dispose() {
			base.Dispose();
		}
	}
	```
1. the [Albatross.Hosting.Utility.BaseOption](https://rushuiguan.github.io/framework/api/Albatross.Hosting.Utility.BaseOption.html) class comes with some commonly used options and command line output methods.   `verbose` and `debug` option interact directly with serilog and therefore changes the logging level.  `clipboard` option will save the output print outs to clipboard when set to true.  `console-out` option will save the output to the file name specified by the parameter.