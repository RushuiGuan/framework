using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// ack msg from a server socket such as router
	/// </summary>
	public record class Heartbeat : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "heartbeat"; }
		public Heartbeat(string route, ulong id) : base(MessageHeader, route, id) { }
		public Heartbeat() { }
	}
}