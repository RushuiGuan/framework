using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// A heartbeat message sent from the DealerClient to RouterServer.
	/// </summary>
	public record class Heartbeat : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "heartbeat"; }
		public Heartbeat(string route, ulong id) : base(MessageHeader, route, id) { }
		public Heartbeat() { }
	}
}