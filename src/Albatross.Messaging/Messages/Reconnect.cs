using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class Reconnect : Message, IMessage {
		public static string MessageHeader => "reconnect";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new Reconnect(route, id);

		public Reconnect(string route, ulong id) : base(MessageHeader, route, id) {
			Route = route;
		}
	}
}
