using Albatross.CommandLine;
using Microsoft.Extensions.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Sample.EFCore.Admin {
	internal class Program {
		static async Task<int> Main(string[] args) {
			if (Microsoft.EntityFrameworkCore.EF.IsDesignTime) {
				new HostBuilder().Build().Run();
				return 0;
			} else {
				return await new MySetup()
				.AddCommands()
				.CommandBuilder
				.Build()
				.InvokeAsync(args);
			}
		}
	}
}