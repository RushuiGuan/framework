using System.CommandLine;
using Albatross.CommandLine;
using System.CommandLine.Parsing;

namespace Test.CommandLine {
	internal class Program {
		static async Task<int> Main(string[] args) {
			var setup = new MySetup().AddCommands();
			var parser = setup.CommandBuilder.Build();
			return await parser.InvokeAsync(args);
		}
	}
}
