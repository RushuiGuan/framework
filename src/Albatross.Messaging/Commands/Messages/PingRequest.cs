using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Commands.Messages {
	/// <summary>
	/// Ping request with id
	/// </summary>
	public record class PingRequest : Message, IMessage {
		public static string MessageHeader => "ping-req";
		public PingRequest(string route, ulong id) : base(MessageHeader, route, id) { }
		public PingRequest() { }
	}
}