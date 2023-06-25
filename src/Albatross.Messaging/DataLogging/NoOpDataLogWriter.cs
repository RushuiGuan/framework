using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.DataLogging {
	public class NoOpDataLogWriter : ILogWriter {
		public void WriteLogEntry(LogEntry logEntry) { }
	}
}