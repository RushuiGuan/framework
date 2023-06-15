using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class LongRunningCommand : Command<int> {
		public int Duration { get; init; }
		public int Counter { get; init; }

		public LongRunningCommand(int duration, int counter) {
			Duration = duration;
			Counter = counter;
		}
	}
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
