using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Messages {
	/// <summary>
	/// a message sent from client to the server to establish a connection
	/// </summary>
	public record class Connect : Message, IMessage, ISystemMessage {
		public static string MessageHeader => "connect";
		public Connect(string route, ulong id) : base(MessageHeader, route, id) { }
		public Connect() { }
	}
}