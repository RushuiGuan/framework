using Albatross.CommandLine;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Core.Commands;
using Sample.Proxy;
using System.CommandLine.Invocation;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sample.Utility {
	[Verb("efficiency-test", typeof(EfficiencyTest))]
	public record class EfficientTestOptions {
		public FileInfo InputFile { get; set; } = null!;
	}
	public class EfficiencyTest : MyBaseHandler<EfficientTestOptions> {
		public EfficiencyTest(CommandProxyService commandProxy, IOptions<EfficientTestOptions> options, ILogger logger)
			: base(commandProxy, options, logger) {
		}
		public override async Task<int> InvokeAsync(InvocationContext context) {
			using var stream = this.options.InputFile.OpenRead();
			var commands = await JsonSerializer.DeserializeAsync<EfficiencyTestComand[]>(stream, Albatross.Serialization.ReducedFootprintJsonSettings.Value.Default);
			if (commands != null) {
				foreach (var item in commands) {
					await this.commandProxy.SubmitSystemCommand(item);
				}
			}
			return 0;
		}
	}
}
