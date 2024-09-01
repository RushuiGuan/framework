using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Albatross.CommandLine;

namespace Sample.CommandLine {
	internal class Program {
		static async Task<int> Main(string[] args) {
			var setup = new MySetup().AddCommands();
			Console.WriteLine("Building Parser");
			var parser = setup.CommandBuilder.Build();
			Console.WriteLine("Adding Commands");
			return await parser.InvokeAsync(args);
		}
	}
}
