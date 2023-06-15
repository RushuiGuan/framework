using Albatross.Messaging.Commands;
using Albatross.Messaging.Eventing;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SampleProject {
	public class PublishCommand : Command {
		public PublishCommand(string topic, int min, int max) {
			Topic = topic;
			Min = min;
			Max = max;
		}

		public string Topic { get; }
		public int Min { get; }
		public int Max { get; }
	}
	public class PublishCommandHandler : BaseCommandHandler<PublishCommand> {
		private readonly ILogger<PublishCommandHandler> logger;
		private readonly IPublisher publisher;

		public PublishCommandHandler(ILogger<PublishCommandHandler> logger, IPublisher publisher) {
			this.logger = logger;
			this.publisher = publisher;
		}

		public override Task Handle(PublishCommand command) {
			for(int i=command.Min; i<=command.Max; i++) {
				publisher.Publish(command.Topic, BitConverter.GetBytes(i));
			}
			return Task.CompletedTask;
		}
	}
}