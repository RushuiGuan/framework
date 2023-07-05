using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Microsoft.Extensions.Logging;
using SampleProject.Commands;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class PublishCommandHandler : BaseCommandHandler<PublishCommand> {
		private readonly ILogger<PublishCommandHandler> logger;
		private readonly IPublisher publisher;

		public PublishCommandHandler(ILogger<PublishCommandHandler> logger, IPublisher publisher) {
			this.logger = logger;
			this.publisher = publisher;
		}

		public override Task Handle(PublishCommand command, string queue) {
			logger.LogInformation("running pub command with topic={topic}, min={min}, max={max}", command.Topic, command.Min, command.Max);
			for(int i=command.Min; i<=command.Max; i++) {
				publisher.Publish(command.Topic, BitConverter.GetBytes(i));
			}
			return Task.CompletedTask;
		}
	}
}