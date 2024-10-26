using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// Reconnect message is sent by the router server to the dealer client when it receives a unregistered client heartbeat
	/// </summary>
	public record class Reconnect : Message, IMessage {
		public static string MessageHeader => "reconnect";
		public Reconnect(string route, ulong id) : base(MessageHeader, route, id) { }
		public Reconnect() { }
	}
}