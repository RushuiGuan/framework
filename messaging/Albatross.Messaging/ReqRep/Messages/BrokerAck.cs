using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System.IO;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class BrokerAck : Message, IMessage {
		public static string MessageHeader => "broker-ack";
		public string Worker { get; private set; } = string.Empty;


		public BrokerAck(string route, ulong id, string worker) : base(MessageHeader, route, id) {
			this.Worker = worker;
		}
		public BrokerAck() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.Worker = msg[index++].Buffer.ToUtf8String();
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(this.Worker);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.Worker = text;
				return;
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(this.Worker);
		}
	}
}