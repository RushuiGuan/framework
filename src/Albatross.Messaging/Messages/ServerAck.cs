using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// ack from server to client
	/// </summary>
	public record class ServerAck : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "server-ack"; }
		public ServerAck(string route, ulong id) : base(MessageHeader, route, id) { }
		public ServerAck() { }
	}
}