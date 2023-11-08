using Albatross.Text;
using Albatross.Messaging.Messages;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandRequest2 : Message, IMessage, ICommandRequest {
		public static string MessageHeader => "cmd-req2";
		public ulong ServerId { get; private set; }
		public string CommandType { get; private set; } = string.Empty;
		public byte[] Payload { get; private set; } = Array.Empty<byte>();

		public CommandRequest2(string route, ulong id, ulong serverId, string commandType, byte[] payload) : base(MessageHeader, route, id) {
			ServerId = serverId;
			CommandType = commandType;
			Payload = payload;
		}
		public CommandRequest2() { }
		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.ServerId = msg[index++].Buffer.ToULong();
			this.CommandType = msg[index++].Buffer.ToUtf8String();
			this.Payload = msg[index++].Buffer;
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendULong(this.ServerId);
			msg.AppendUtf8String(this.CommandType);
			msg.Append(this.Payload);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				if (ulong.TryParse(text, out var value)) {
					this.ServerId = value;
					if (line.TryGetText(LogDelimiter, ref offset, out text)) {
						this.CommandType = text;
						if (line.TryGetText(LogDelimiter, ref offset, out text)) {
							this.Payload = Decode(text);
							return;
						}
					}
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(ServerId)
				.Space().Append(CommandType)
				.Space().Append(Encode(Payload));
		}
	}
}