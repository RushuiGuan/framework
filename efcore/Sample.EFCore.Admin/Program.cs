using Albatross.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	internal class Program {
		static Task<int> Main(string[] args) =>
			new MySetup()
				.AddCommands()
				.CommandBuilder
				.Build()
				.InvokeAsync(args);
	}
}
