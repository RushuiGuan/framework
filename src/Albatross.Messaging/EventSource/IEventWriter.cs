namespace Albatross.Messaging.EventSource {
	public interface IEventWriter {
		void WriteLogEntry(LogEntry logEntry);
	}
}
