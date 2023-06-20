using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class Reconnect : Message, IMessage {
		public static string MessageHeader => "reconnect";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) => new Reconnect(route);

		public Reconnect(string route) : base(MessageHeader, route, 0) {
			Route = route;
		}
	}
}
