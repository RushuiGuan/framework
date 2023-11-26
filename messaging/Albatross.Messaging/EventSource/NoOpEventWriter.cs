using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.EventSource {
	public class NoOpEventWriter : IEventWriter {
		public void WriteEvent(EventEntry logEntry) { }
	}
}