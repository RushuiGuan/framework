using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Eventing.Messages {
	public record class SubscriptionReply : Message, IMessage {
		public static string MessageHeader => "sub-rep";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			var on = frames.PopBoolean();
			var pattern = frames.PopUtf8String();
			return new SubscriptionReply(route, id, on, pattern);
		}

		public SubscriptionReply(string route, ulong id, bool on, string pattern) : base(MessageHeader, route, id) {
			this.On = on;
			this.Pattern = pattern;
		}

		public bool On { get; init; }
		public string Pattern { get; init; }


		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendBoolean(this.On);
			msg.AppendUtf8String(this.Pattern);
			return msg;
		}
	}
}