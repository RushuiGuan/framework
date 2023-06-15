using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Messaging.Eventing.Messages {
	public record class SubscriptionReply : Message, IMessage {
		public static string MessageHeader => "sub-rep";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			var on = frames.PopBoolean();
			List<string> topics = new List<string>();
			while (frames.Any()) {
				topics.Add(frames.PopUtf8String());
			}
			return new SubscriptionReply(route, id, on, topics);
		}

		public SubscriptionReply(string route, ulong id, bool on, IEnumerable<string> topics) : base(MessageHeader, route, id) {
			this.On = on;
			Topics = topics;
		}

		public IEnumerable<string> Topics { get; init; }
		public bool On { get; init; }

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendBoolean(this.On);
			foreach (var topic in Topics) {
				msg.AppendUtf8String(topic);
			}
			return msg;
		}
	}
}
