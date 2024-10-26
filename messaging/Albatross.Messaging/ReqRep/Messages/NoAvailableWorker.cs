using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System.IO;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class NoAvailableWorker : Message, IMessage {
		public static string MessageHeader => "no-worker";
		public string Service { get; private set; } = string.Empty;

		public NoAvailableWorker(string route, ulong messageId, string service) : base(MessageHeader, route, messageId) {
			Service = service;
		}
		public NoAvailableWorker() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.Service = msg[index++].Buffer.ToUtf8String();
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(this.Service);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.Service = text;
				return;
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(this.Service);
		}
	}
}