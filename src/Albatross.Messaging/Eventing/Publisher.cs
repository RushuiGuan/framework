using Albatross.Messaging.Services;

namespace Albatross.Messaging.Eventing {
	public interface IPublisher  {
		void Publish(string topic, byte[] payload);
	}
	public class Publisher : IPublisher {
		private readonly RouterServer routerServer;

		public Publisher(RouterServer routerServer) {
			this.routerServer = routerServer;
		}
		public void Publish(string topic, byte[] payload) => routerServer.SubmitToQueue(new PubEvent(topic, payload));
	}
}
