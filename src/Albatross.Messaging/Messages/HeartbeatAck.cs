using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class HeartbeatAck : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "heartbeat-ack"; }
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new Ack(endpoint, id);

		public HeartbeatAck(string route) : base(MessageHeader, route, 0) { }
	}
}