using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class UnstableCommand : Command<int> {
		public int Counter { get; }
		public UnstableCommand(int counter) {
			this.Counter = counter;
		}
	}
	public class UnstableCommandHandler : BaseCommandHandler<UnstableCommand, int> {
		private readonly ILogger<UnstableCommandHandler> logger;

		public UnstableCommandHandler(ILogger<UnstableCommandHandler> logger) {
			this.logger = logger;
		}
		public override Task<int> Handle(UnstableCommand command) {
			logger.LogInformation("{value}, {thread}", command.Counter, Environment.CurrentManagedThreadId);
			var value = new Random().Next();
			if (value % 2 == 0) {
				throw new Exception(command.Counter.ToString());
			}
			return Task.FromResult(command.Counter);
		}
	}
}