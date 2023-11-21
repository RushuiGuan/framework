using Albatross.Messaging.Commands;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class TestExceptionCommandHandler : BaseCommandHandler<TestExceptionCommand> {
		private readonly ILogger<TestExceptionCommandHandler> logger;

		public TestExceptionCommandHandler(ILogger<TestExceptionCommandHandler> logger) {
			this.logger = logger;
		}

		public override Task Handle(TestExceptionCommand command) {
			logger.LogInformation("this is a normal msg with value {value}", 2);
			logger.LogError("error message with {variable} value", 1);
			logger.LogError(new Exception("test"), "error msg with {variable}", 1);
			throw new InvalidOperationException("my own error");
		}
	}
}
