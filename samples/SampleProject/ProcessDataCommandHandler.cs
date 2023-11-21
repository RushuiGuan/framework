using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Threading.Tasks;

namespace SampleProject {
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
