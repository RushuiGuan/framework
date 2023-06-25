namespace Albatross.Messaging.Messages {
	/// <summary>
	/// ack from client ot server
	/// </summary>
	public record class ClientAck : Message, IMessage {
		public static string MessageHeader { get => "client-ack"; }
		public ClientAck(string route, ulong id) : base(MessageHeader, route, id) { }
		public ClientAck() { }
	}
}