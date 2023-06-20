using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Eventing.Messages {
	public record class Event : Message, IMessage {
		public static string MessageHeader => "eve";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			var topic = frames.PopUtf8String();
			var pattern = frames.PopUtf8String();
			var payload = frames.Pop().Buffer;
			return new Event(route, id, topic, pattern, payload);
		}
		public string Topic { get; init; }
		public string Pattern { get; init; }
		public byte[] Payload { get; init; }

		public Event(string route, ulong id, string topic, string pattern, byte[] payload) : base(MessageHeader, route, id) {
			Topic = topic;
			Pattern = pattern;
			Payload = payload;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendUtf8String(Topic);
			msg.AppendUtf8String(Pattern);
			msg.Append(Payload);
			return msg;
		}
	}
}