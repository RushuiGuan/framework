using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class BrokerReconnect : Message, IMessage {
		public static string MessageHeader => "broker-reconnect";

		public BrokerReconnect(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
		public BrokerReconnect() { }
	}
}
