namespace Albatross.Messaging.Eventing {
	public interface IPublisher {
		void Publish(string topic, byte[] payload);
	}
}
