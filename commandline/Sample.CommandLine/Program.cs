using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Albatross.CommandLine;

namespace Sample.CommandLine {
	internal class Program {
		static Task<int> Main(string[] args) =>
			new MySetup()
				.AddCommands()
				.CommandBuilder
				.Build()
				.InvokeAsync(args);
	}
}
