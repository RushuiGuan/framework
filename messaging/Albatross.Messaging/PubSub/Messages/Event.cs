using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System;
using System.IO;
using System.Web;

namespace Albatross.Messaging.PubSub.Messages {
	public record class Event : Message, IMessage {
		public static string MessageHeader => "eve";
		public string Topic { get; private set; } = string.Empty;
		public string Pattern { get; private set; } = string.Empty;
		public byte[] Payload { get; private set; } = Array.Empty<byte>();

		public Event(string route, ulong id, string topic, string pattern, byte[] payload) : base(MessageHeader, route, id) {
			Topic = topic;
			Pattern = pattern;
			Payload = payload;
		}
		public Event() { }

		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(Topic);
			msg.AppendUtf8String(Pattern);
			msg.Append(Payload);
		}
		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = this.StartingFrameIndex;
			this.Topic = msg[index++].Buffer.ToUtf8String();
			this.Pattern = msg[index++].Buffer.ToUtf8String();
			this.Payload = msg[index++].Buffer;
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.Topic = HttpUtility.UrlDecode(text);
				if (line.TryGetText(LogDelimiter, ref offset, out text)) {
					this.Pattern = HttpUtility.UrlDecode(text);
					if (line.TryGetText(LogDelimiter, ref offset, out text)) {
						this.Payload = this.Decode(text);
						return;
					}
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(HttpUtility.UrlEncode(Topic))
				.Space().Append(HttpUtility.UrlEncode(Pattern))
				.Space().Append(Encode(this.Payload));
		}
	}
}