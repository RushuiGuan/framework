using Albatross.Text;
using Albatross.Messaging.Messages;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandQueueStatusReply : Message, IMessage {
		public static string MessageHeader => "queue-status-reply";
		public byte[] Payload { get; private set; }

		public CommandQueueStatusReply(string route, ulong id, byte[] payload) : base(MessageHeader, route, id) {
			Payload = payload;
		}

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			this.Payload = msg[base.StartingFrameIndex].Buffer;
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if(line.TryGetText(LogDelimiter, ref offset, out var text)) {
				Payload = Decode(text);
				return;
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.Append(Payload);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(Encode(Payload));
		}
	}
}
