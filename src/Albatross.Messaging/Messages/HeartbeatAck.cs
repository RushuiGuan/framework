using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class HeartbeatAck : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "heartbeat-ack"; }
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) => new HeartbeatAck(endpoint, id);

		public HeartbeatAck(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}