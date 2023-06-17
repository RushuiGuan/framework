using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class Reconnect : Message, IMessage {
		public static string MessageHeader => "reconnect";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) => new Reconnect(route, messageId);

		public Reconnect(string route, ulong messageId) : base(MessageHeader, route, messageId) {
			Route = route;
		}
	}
}
