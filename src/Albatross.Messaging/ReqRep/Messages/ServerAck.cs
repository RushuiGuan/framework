using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// ack msg from a server socket such as router
	/// </summary>
	public record class ServerAck : Message, IMessage {
		public static string MessageHeader { get => "server-ack"; }
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new ServerAck(endpoint, id);

		public ServerAck(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}