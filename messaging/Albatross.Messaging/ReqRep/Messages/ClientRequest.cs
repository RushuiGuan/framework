using Albatross.Messaging.Messages;
using Albatross.Text;
using NetMQ;
using System;
using System.IO;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class ClientRequest : Message, IMessage {
		public static string MessageHeader => "client-req";
		public string Service { get; private set; } = string.Empty;
		public byte[] Payload { get; private set; } = Array.Empty<byte>();

		public ClientRequest(string route, ulong id, string service, byte[] payload) : base(MessageHeader, route, id) {
			this.Service = service;
			this.Payload = payload;
		}
		public ClientRequest() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.Service = msg[index++].Buffer.ToUtf8String();
			this.Payload = msg[index++].Buffer;
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(this.Service);
			msg.Append(this.Payload);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.Service = text;
				if (line.TryGetText(LogDelimiter, ref offset, out text)) {
					this.Payload = Decode(text);
					return;
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(this.Service)
				.Space().Append(Encode(Payload));
		}
	}
}