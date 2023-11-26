using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// A heartbeat response sent from the RouterServer to DealerClient
	/// </summary>
	public record class HeartbeatAck : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "heartbeat-ack"; }
		public HeartbeatAck(string route, ulong id) : base(MessageHeader, route, id) { }
		public HeartbeatAck() { }
	}
}