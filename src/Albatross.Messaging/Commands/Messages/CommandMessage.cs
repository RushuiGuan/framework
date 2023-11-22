using Albatross.Text;
using Albatross.Messaging.Messages;
using NetMQ;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandMessage : Message{
		public string CommandType { get; private set; } = string.Empty;
		public override int StartingFrameIndex => base.StartingFrameIndex + 1;
		public CommandMessage(string messageHeader, string route, ulong id, string commandType) : base(messageHeader, route, id) {
			CommandType = commandType;
		}
		public CommandMessage() { }
		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.CommandType = msg[index++].Buffer.ToUtf8String();
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(this.CommandType);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.CommandType = text;
				return;
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(CommandType);
		}
	}
}