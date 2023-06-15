using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Eventing.Messages {
	public record class Event : Message, IMessage {
		public static string MessageHeader => "eve";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			var topic = frames.PopUtf8String();
			var payload = frames.Pop().Buffer;
			return new Event(route, id, topic, payload);
		}
		public string Topic { get; init; }
		public byte[] Payload { get; init; }

		public Event(string route, ulong id, string topic, byte[] payload) : base(MessageHeader, route, id) {
			Topic = topic;
			Payload = payload;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendUtf8String(Topic);
			msg.Append(Payload);
			return msg;
		}
	}
}