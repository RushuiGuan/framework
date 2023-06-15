using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class ConnectOk : Message, IMessage {
		public static string MessageHeader => "connect-ok";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new ConnectOk(route, id);
		public ConnectOk(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
	}
}