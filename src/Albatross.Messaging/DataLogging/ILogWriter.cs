namespace Albatross.Messaging.DataLogging {
	public interface ILogWriter {
		void WriteLogEntry(LogEntry logEntry);
	}
}
