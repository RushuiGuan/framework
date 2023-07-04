using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// Ack from server to client. This is a generic ack used\shared by protocols.  It is not used by the services
	/// </summary>
	public record class ServerAck : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "server-ack"; }
		public ServerAck(string route, ulong id) : base(MessageHeader, route, id) { }
		public ServerAck() { }
	}
}