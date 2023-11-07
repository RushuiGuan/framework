namespace Albatross.Messaging.EventSource {
	public interface IEventWriter {
		void WriteLogEntry(EventEntry logEntry);
	}
}
