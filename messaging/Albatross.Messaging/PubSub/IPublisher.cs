namespace Albatross.Messaging.PubSub {
	public interface IPublisher {
		void Publish(string topic, byte[] payload);
	}
}