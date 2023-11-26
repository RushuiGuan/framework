using Albatross.Text;
using Albatross.Messaging.Messages;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandRequest : Message, IMessage {
		public static string MessageHeader => "cmd-req";
		public string CommandType { get; private set; } = string.Empty;
		public CommandMode Mode { get; private set; }
		public byte[] Payload { get; private set; } = Array.Empty<byte>();

		public CommandRequest(string route, ulong id, string commandType, CommandMode mode, byte[] payload) : base(MessageHeader, route, id) {
			CommandType = commandType;
			Mode = mode;
			Payload = payload;
		}
		public CommandRequest() { }
		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.CommandType = msg[index++].Buffer.ToUtf8String();
			this.Mode = (CommandMode)BitConverter.ToInt16(msg[index++].Buffer);
			this.Payload = msg[index++].Buffer;
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(this.CommandType);
			msg.AppendInt16((short)this.Mode);
			msg.Append(this.Payload);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.CommandType = text;
				if (line.TryGetText(LogDelimiter, ref offset, out text) && Enum.TryParse<CommandMode>(text, out var mode)) {
					this.Mode = mode;
					if (line.TryGetText(LogDelimiter, ref offset, out text)) {
						this.Payload = Decode(text);
						return;
					}
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(CommandType)
				.Space().Append(this.Mode)
				.Space().Append(Encode(Payload));
		}
	}
}