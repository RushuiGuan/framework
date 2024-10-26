using Albatross.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Benchmark.Collections {
	internal class Program {
		static Task<int> Main(string[] args) {
			return new MySetup().AddCommands().CommandBuilder.Build().InvokeAsync(args);
		}
	}
}