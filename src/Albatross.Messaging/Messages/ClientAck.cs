using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// ack from client ot server
	/// </summary>
	public record class ClientAck : Message, IMessage {
		public static string MessageHeader { get => "client-ack"; }
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new ClientAck(endpoint, id);

		public ClientAck(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}