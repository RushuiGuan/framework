using Albatross.Messaging.Messages;
using NetMQ;
using System;

namespace Albatross.Messaging.DataLogging {
	public interface IDataLogWriter {
		void Outgoing(IMessage message, NetMQMessage frames);
		void Incoming(string route, string header, ulong messageId, NetMQMessage frames);
		void Record(IMessage message);
	}
}
