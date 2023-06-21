using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Eventing.Messages {
	public record class UnsubscribeAllRequest : Message, IMessage {
		public static string MessageHeader => "unsub-all";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			return new UnsubscribeAllRequest(route, id);
		}
		public UnsubscribeAllRequest(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}
