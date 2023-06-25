using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class ConnectOk : Message, IMessage, ISystemMessage {
		public static string MessageHeader => "connect-ok";
		public ConnectOk(string route, ulong id) : base(MessageHeader, route, id) { }
		public ConnectOk() { }
	}
}