using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.DataLogging {
	public class NoOpDataLogWriter : IDataLogWriter {
		public void Dispose() { }
		public void Incoming(string route, string header, ulong messageId, NetMQMessage frames) { }
		public void Outgoing(IMessage message, NetMQMessage frames) { }
		public void Record(IMessage message) { }
	}
}
