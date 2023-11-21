using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class LongRunningCommandHandler : BaseCommandHandler<LongRunningCommand, int> {
		private readonly ILogger<LongRunningCommandHandler> logger;

		public LongRunningCommandHandler(ILogger<LongRunningCommandHandler> logger) {
			this.logger = logger;
		}

		public override async Task<int> Handle(LongRunningCommand command) {
			logger.LogInformation("{counter}: {thread}", command.Counter, Environment.CurrentManagedThreadId);
			await Task.Delay(command.Duration);
			return command.Counter;
		}
	}
}
