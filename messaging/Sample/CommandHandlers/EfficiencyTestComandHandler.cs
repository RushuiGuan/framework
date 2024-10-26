using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Core.Commands;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sample.CommandHandlers {
	public class EfficiencyTestComandHandler : BaseCommandHandler<EfficiencyTestComand> {
		private readonly ILogger<EfficiencyTestComandHandler> logger;
		private readonly ICommandClient client;

		public EfficiencyTestComandHandler(ILogger<EfficiencyTestComandHandler> logger, InternalCommandClient client) {
			this.logger = logger;
			this.client = client;
		}

		public override async Task Handle(EfficiencyTestComand command) {
			if (command.CPUBound) {
				logger.LogInformation("Running cpu bound task");
				Stopwatch stopwatch = Stopwatch.StartNew();
				int counter = 0;
				while (stopwatch.Elapsed.TotalMilliseconds < command.Duration) {
					counter++;
					if (counter % 10 == 0) {
						logger.LogDebug("CPUIntensiveTask: {counter}", counter);
					}
				}
			} else {
				logger.LogInformation("Running io bound task");
				await Task.Delay(command.Duration);
			}

			if (command.SubCommandCount > 0 && command.SubCommand != null) {
				for (int i = 0; i < command.SubCommandCount; i++) {
					_ = this.client.Submit(command.SubCommand with { }, true, 0);
				}
			}
		}
	}
}