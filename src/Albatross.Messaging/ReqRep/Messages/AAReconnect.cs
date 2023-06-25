using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class AAReconnect : Message, IMessage {
		public static string MessageHeader => "aareconnect";

		public AAReconnect(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
	}
}
