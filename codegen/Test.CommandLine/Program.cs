using Albatross.CommandLine;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace Test.CommandLine {
	internal class Program {
		static Task<int> Main(string[] args) {
			return new MySetup().AddCommands().CommandBuilder.Build().InvokeAsync(args);
		}
	}
}