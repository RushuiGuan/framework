using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandMessage : Message {
		public string CommandName { get; private set; } = string.Empty;
		public override int StartingFrameIndex => base.StartingFrameIndex + 1;
		public CommandMessage(string messageHeader, string route, ulong id, string commandName) : base(messageHeader, route, id) {
			CommandName = commandName;
		}
		public CommandMessage() { }
		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.CommandName = msg[index++].Buffer.ToUtf8String();
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(this.CommandName);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.CommandName = text;
				return;
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(CommandName);
		}
	}
}