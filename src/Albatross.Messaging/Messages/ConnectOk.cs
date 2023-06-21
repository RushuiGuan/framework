using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class ConnectOk : Message, IMessage, ISystemMessage {
		public static string MessageHeader => "connect-ok";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new ConnectOk(route, id);
		public ConnectOk(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}