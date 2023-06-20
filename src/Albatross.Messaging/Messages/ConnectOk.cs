using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class ConnectOk : Message, IMessage, ISystemMessage {
		public static string MessageHeader => "connect-ok";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new ConnectOk(route);
		public ConnectOk(string route) : base(MessageHeader, route, 0) { }
	}
}