using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System;
using System.IO;
using System.Web;

namespace Albatross.Messaging.PubSub.Messages {
	public record class SubscriptionReply : Message, IMessage {
		public static string MessageHeader => "sub-rep";
		public SubscriptionReply(string route, ulong id, bool on, string pattern) : base(MessageHeader, route, id) {
			this.On = on;
			this.Pattern = pattern;
		}
		public SubscriptionReply() { }

		public bool On { get; private set; }
		public string Pattern { get; private set; } = string.Empty;

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = this.StartingFrameIndex;
			this.On = msg[index++].Buffer.ToBoolean();
			this.Pattern = msg[index++].Buffer.ToUtf8String();
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.Append(BitConverter.GetBytes(this.On));
			msg.Append(this.Pattern.ToUtf8Bytes());
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text) && bool.TryParse(text, out var booleanValue)) {
				this.On = booleanValue;
				if (line.TryGetText(LogDelimiter, ref offset, out text)) {
					this.Pattern = HttpUtility.UrlDecode(text);
					return;
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(this.On)
				.Space().Append(HttpUtility.UrlEncode(this.Pattern));
		}
	}
}