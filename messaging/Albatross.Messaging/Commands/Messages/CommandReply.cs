using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandReply : CommandMessage, IMessage {
		public static string MessageHeader => "cmd-rep";
		public byte[] Payload { get; private set; } = Array.Empty<byte>();

		public CommandReply(string route, ulong id, string commandName, byte[] payload) : base(MessageHeader, route, id, commandName) {
			Payload = payload;
		}
		public CommandReply() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			this.Payload = msg[base.StartingFrameIndex].Buffer;
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
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