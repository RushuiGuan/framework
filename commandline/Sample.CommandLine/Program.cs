using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

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