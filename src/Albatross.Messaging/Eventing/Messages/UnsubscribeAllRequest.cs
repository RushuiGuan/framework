using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Eventing.Messages {
	public record class UnSubscribeAllRequest : Message, IMessage {
		public static string MessageHeader => "sub";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			var on = frames.PopBoolean();
			var isRegex = frames.PopBoolean();
			var topic = frames.PopUtf8String();
			return new UnSubscribeAllRequest(route, id, on, isRegex, topic);
		}

		public UnSubscribeAllRequest(string route, ulong id, bool on, bool isRegex, string topic) : base(MessageHeader, route, id) {
			this.On = on;
			this.IsRegex = isRegex;
			this.Topic = topic;
		}

		public bool On { get; init; }
		public bool IsRegex { get; init; }
		public string Topic { get; init; }

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendBoolean(this.On);
			msg.AppendBoolean(this.IsRegex);
			msg.AppendUtf8String(this.Topic);
			return msg;
		}
	}
}
