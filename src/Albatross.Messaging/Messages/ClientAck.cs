namespace Albatross.Messaging.Messages {
	/// <summary>
	/// Ack from client to server. This is a generic ack used\shared by protocols.  It is not used by the services
	/// </summary>
	public record class ClientAck : Message, IMessage {
		public static string MessageHeader { get => "client-ack"; }
		public ClientAck(string route, ulong id) : base(MessageHeader, route, id) { }
		public ClientAck() { }
	}
}