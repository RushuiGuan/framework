using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Eventing.Messages {
	public record class UnSubscribeAllRequest : Message, IMessage {
		public static string MessageHeader => "unsub-all";

		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new UnSubscribeAllRequest(route, id);
		public UnSubscribeAllRequest(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}
