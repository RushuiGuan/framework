using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	[Command]
	public class FireAndForgetCommand {
		public int Counter { get; }
		public int? Duration { get; }
		public FireAndForgetCommand(int counter, int? duration) {
			this.Counter = counter;
			Duration = duration;
		}
	}

	public class FireAndForgetCommandHandler : BaseCommandHandler<FireAndForgetCommand> {
		public const int DefaultDuration = 150;
		private readonly ILogger<FireAndForgetCommand> logger;

		public FireAndForgetCommandHandler(ILogger<FireAndForgetCommand> logger) {
			this.logger = logger;
		}

		public override async Task Handle(FireAndForgetCommand command, string queue) {
			logger.LogInformation("executing fire and forget command: {value}, {thread}", command.Counter, Environment.CurrentManagedThreadId);
			await Task.Delay(command.Duration ?? DefaultDuration);
		}
	}
}
