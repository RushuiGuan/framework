using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// ack msg from a server socket such as router
	/// </summary>
	public record class AAServerAck : Message, IMessage {
		public static string MessageHeader { get => "aaserver-ack"; }
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new ServerAck(endpoint, id);

		public AAServerAck(string route, ulong id) : base(MessageHeader, route, id) { }
		public AAServerAck() { }	
	}
}