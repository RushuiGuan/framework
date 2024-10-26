using Albatross.CodeGen.CommandLine;
using Albatross.CommandLine;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Tests.Utility {
	internal class Program {
		static async Task<int> Main(string[] args) {
			var setup = new MySetup().AddCommands();
			var parser = setup.CommandBuilder.Build();
			return await parser.InvokeAsync(args);
		}
	}
}