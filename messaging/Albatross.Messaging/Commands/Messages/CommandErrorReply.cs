using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandErrorReply : CommandMessage, IMessage {
		public static string MessageHeader => "cmd-err";
		public string ClassName { get; private set; } = string.Empty;
		public byte[] Message { get; private set; } = Array.Empty<byte>();

		public CommandErrorReply(string route, ulong id, string commandName, string errorClassName, byte[] message) : base(MessageHeader, route, id, commandName) {
			ClassName = errorClassName;
			Message = message;
		}
		public CommandErrorReply() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = this.StartingFrameIndex;
			this.ClassName = msg[index++].Buffer.ToUtf8String();
			this.Message = msg[index++].Buffer;
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(ClassName);
			msg.Append(this.Message);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.ClassName = text;
				if (line.TryGetText(LogDelimiter, ref offset, out text)) {
					this.Message = Decode(text);
					return;
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(ClassName)
				.Space().Append(Encode(Message));
		}
	}
}