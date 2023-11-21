using Albatross.Messaging.Messages;

namespace Albatross.Messaging.Test {
	/// <summary>
	/// Ack from server to client. This is a generic ack used\shared by protocols.  It is not used by the services
	/// </summary>
	public record class TestMsg : Message, IMessage, ISystemMessage {
		public static string MessageHeader { get => "test"; }
		public TestMsg(string route, ulong id) : base(MessageHeader, route, id) { }
		public TestMsg() { }
		public const int MessageSize = 30;
	}
}