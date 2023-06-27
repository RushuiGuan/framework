using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	[Command(typeof(long))]
	public class DoMathWorkCommand {
		public long Counter { get; init; }

		public DoMathWorkCommand(long counter) {
			this.Counter = counter;
		}
	}
	public class DoMathWorkCommandHandler : BaseCommandHandler<DoMathWorkCommand, long> {
		private readonly ILogger<DoMathWorkCommandHandler> logger;

		public DoMathWorkCommandHandler(ILogger<DoMathWorkCommandHandler> logger) {
			this.logger = logger;
		}

		public override Task<long> Handle(DoMathWorkCommand command) {
			logger.LogInformation("{counter}: {thread}", command.Counter, Environment.CurrentManagedThreadId);
			return Task.FromResult(command.Counter + 1);
		}
	}
}
