using Albatross.Hosting.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace Sample.CommandLine {
	internal class Program {
		static Task<int> Main(string[] args)
			=> new Setup().CreateCommandBuilder().UseDefaults().Build().InvokeAsync(args);
	}
}
