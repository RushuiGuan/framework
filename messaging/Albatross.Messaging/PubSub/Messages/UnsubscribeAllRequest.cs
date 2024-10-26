using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.PubSub.Messages {
	public record class UnsubscribeAllRequest : Message, IMessage {
		public static string MessageHeader => "unsub-all";
		public UnsubscribeAllRequest(string route, ulong id) : base(MessageHeader, route, id) { }
		public UnsubscribeAllRequest() { }
	}
}