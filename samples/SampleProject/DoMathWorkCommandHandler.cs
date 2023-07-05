using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class DoMathWorkCommandHandler : BaseCommandHandler<DoMathWorkCommand, long> {
		private readonly ILogger<DoMathWorkCommandHandler> logger;

		public DoMathWorkCommandHandler(ILogger<DoMathWorkCommandHandler> logger) {
			this.logger = logger;
		}

		public override Task<long> Handle(DoMathWorkCommand command, string queue) {
			logger.LogInformation("{counter}: {thread}", command.Counter, Environment.CurrentManagedThreadId);
			return Task.FromResult(command.Counter + 1);
		}
	}
}
