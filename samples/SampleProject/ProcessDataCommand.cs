using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	[Command(typeof(long))]
	public class ProcessDataCommand {
		public long Counter { get; init; }

		public ProcessDataCommand(long counter) {
			this.Counter = counter;
		}
	}
	public class ProcessDataCommandHandler : BaseCommandHandler<ProcessDataCommand, long> {
		private readonly ILogger<ProcessDataCommandHandler> logger;

		public ProcessDataCommandHandler(ILogger<ProcessDataCommandHandler> logger) {
			this.logger = logger;
		}
		public override Task<long> Handle(ProcessDataCommand command) {
			logger.LogInformation("{value}, {thread}", command.Counter, Environment.CurrentManagedThreadId);
			return Task.FromResult(command.Counter - 1);
		}
	}
}
