﻿using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	[Command(typeof(int))]
	public class UnstableCommand {
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
			throw new Exception("this job has been failed successfully");
		}
	}
}