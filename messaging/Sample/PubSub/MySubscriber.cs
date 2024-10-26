using Albatross.Messaging.PubSub;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Sample.PubSub {
	public class MySubscriber : ISubscriber {
		private readonly ILogger<MySubscriber> logger;

		public MySubscriber(ILogger<MySubscriber> logger) {
			this.logger = logger;
		}
		public Task DataReceived(string topic, byte[] data) {
			var text = BitConverter.ToInt32(data);
			logger.LogInformation("received {topic}:{data}", topic, text);
			return Task.CompletedTask;
		}

		public bool Equals(ISubscriber other) => ReferenceEquals(this, other);
	}
}