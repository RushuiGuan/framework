using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using Sample.Messaging.Commands;
using System;
using System.Threading.Tasks;

namespace Sample.Messaging {
	public class SelfDestructCommandHandler : BaseCommandHandler<SelfDestructCommand> {
		private readonly ILogger<SelfDestructCommandHandler> logger;

		public SelfDestructCommandHandler(ILogger<SelfDestructCommandHandler> logger) {
			this.logger = logger;
		}

		public override async Task Handle(SelfDestructCommand command) {
			var selfDestruct = false;
			var diff = DateTime.Now.Ticks - command.Tick;
			logger.LogInformation("clock is {diff}", diff);
			var gap = 10000000;
			if (diff < gap) {
				selfDestruct = true;
			}
			if(selfDestruct) {
				logger.LogInformation("Self destructing in {delay} second", command.Delay);
				if (command.Delay > 0) {
					await Task.Delay(command.Delay * 1000);
				}
				System.Diagnostics.Process.GetCurrentProcess().Kill();
			} else {
				logger.LogInformation("Skip because {diff} >= {gap}", diff, gap);
			}
		}
	}
}
