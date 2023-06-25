using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class Connect : Message, IMessage, ISystemMessage {
		public static string MessageHeader => "connect";
		public Connect(string route, ulong id) : base(MessageHeader, route, id) { }
		public Connect() { }
	}
}