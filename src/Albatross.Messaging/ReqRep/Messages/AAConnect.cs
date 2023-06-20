using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class AAConnect : Message, IMessage {
		public static string MessageHeader => "aaconnect";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new AAConnect(route, id);
		public AAConnect(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
	}
}