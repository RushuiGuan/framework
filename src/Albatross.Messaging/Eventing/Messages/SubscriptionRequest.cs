using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Eventing.Messages {
	public record class SubscriptionRequest : Message, IMessage {
		public static string MessageHeader => "sub";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			var on = frames.PopBoolean();
			var topic = frames.PopUtf8String();
			return new SubscriptionRequest(route, id, on, topic);
		}

		public SubscriptionRequest(string route, ulong id, bool on, string topic) : base(MessageHeader, route, id) {
			this.On = on;
			this.Topic = topic;
		}

		public bool On { get; init; }
		public string Topic { get; init; }

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendBoolean(this.On);
			msg.AppendUtf8String(this.Topic);
			return msg;
		}
	}
}
