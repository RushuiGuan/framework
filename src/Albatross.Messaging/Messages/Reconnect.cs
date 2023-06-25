using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class Reconnect : Message, IMessage {
		public static string MessageHeader => "reconnect";
		public Reconnect(string route, ulong id) : base(MessageHeader, route, id) { }
		public Reconnect() { }
	}
}
