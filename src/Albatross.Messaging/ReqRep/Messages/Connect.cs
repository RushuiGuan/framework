using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class Connect : Message, IMessage {
		public static string MessageHeader => "connect";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new Connect(route, id);
		public Connect(string route, ulong messageId) : base(MessageHeader, route, messageId) { }
	}
}