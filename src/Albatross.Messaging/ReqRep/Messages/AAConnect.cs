using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class AAConnect : Message, IMessage {
		public static string MessageHeader => "aaconnect";
		public AAConnect(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
		public AAConnect() { }
	}
}