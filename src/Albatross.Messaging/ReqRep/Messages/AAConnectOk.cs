using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class AAConnectOk : Message, IMessage {
		public static string MessageHeader => "aaconnect-ok";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new AAConnectOk(route, id);
		public AAConnectOk(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
	}
}