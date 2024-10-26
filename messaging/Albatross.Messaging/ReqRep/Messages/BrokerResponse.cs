using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System;
using System.IO;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class BrokerResponse : Message, IMessage {
		public static string MessageHeader => "broker-rep";
		public byte[] Payload { get; private set; } = Array.Empty<byte>();

		public BrokerResponse(string route, ulong messageId, byte[] payload) : base(MessageHeader, route, messageId) {
			this.Payload = payload;
		}
		public BrokerResponse() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.Payload = msg[index++].Buffer;
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.Append(this.Payload);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.Payload = Decode(text);
				return;
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space()
				.Space().Append(Encode(Payload));
		}
	}
}