using Albatross.Hosting.Utility;
using CommandLine;
using System.Threading.Tasks;

namespace Sample.Hosting.Utility {
	internal class Program {
		static Task<int> Main(string[] args) {
			// return Parser.Default.Run(args, typeof(Program).Assembly);
			return new CommandBuilder()
				.AddUtility<ShowEnvironmentOption, ShowEnvironment>()
				.AddUtility<HelloWorldOption, HelloWorld>()
				.Run(Parser.Default, args);
		}
	}
}
