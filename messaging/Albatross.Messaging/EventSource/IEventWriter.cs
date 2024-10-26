namespace Albatross.Messaging.EventSource {
	public interface IEventWriter {
		void WriteEvent(EventEntry logEntry);
	}
}