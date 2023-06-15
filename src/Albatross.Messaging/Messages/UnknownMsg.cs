using NetMQ;
using System;

namespace Albatross.Messaging.Messages {
	public record class UnknownMsg : Message, IMessage {
		public static string MessageHeader => "unknown";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => throw new NotSupportedException();

		public NetMQMessage Payload { get; }

		public UnknownMsg(string header, string route, ulong id, NetMQMessage payload) : base(header, route, id) {
			Payload = payload;
		}
	}
}