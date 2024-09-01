using System.CommandLine;
using Albatross.CommandLine;
using Albatross.CodeGen.CommandLine;
using System.Threading.Tasks;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace Albatross.CodeGen.Tests.Utility {
	internal class Program {
		static async Task<int> Main(string[] args) {
			var setup = new MySetup().AddCommandHandlers();
			var parser = setup.CommandBuilder.Build();
			return await parser.InvokeAsync(args);
		}
	}
}
