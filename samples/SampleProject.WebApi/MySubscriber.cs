using Albatross.Messaging.Eventing;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.WebApi {
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
	}
}
