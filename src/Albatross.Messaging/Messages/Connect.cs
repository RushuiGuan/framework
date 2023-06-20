using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Messages {
	public record class Connect : Message, IMessage, ISystemMessage {
		public static string MessageHeader => "connect";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new Connect(route);
		public Connect(string route) : base(MessageHeader, route, 0) { }
	}
}