using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Sample.Utility {
	public class Program {
		static Task<int> Main(string[] args) {
			return new MySetup().AddCommands().CommandBuilder.Build().InvokeAsync(args);
		}
	}
}