using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class AAConnectOk : Message, IMessage {
		public static string MessageHeader => "aaconnect-ok";
		public AAConnectOk(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
		public AAConnectOk() { }
	}
}