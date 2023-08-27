using Albatross.Text;
using Albatross.Messaging.Messages;
using NetMQ;
using System.Collections.Generic;
using System.IO;

namespace Albatross.Messaging.ReqRep.Messages {
	/// <summary>
	/// Messages for worker to connect to broker
	/// </summary>
	public record class WorkerConnect : Message, IMessage {
		public static string MessageHeader => "worker-connect";
		public ISet<string> Services { get; } = new HashSet<string>();

		public WorkerConnect(string route, ulong messageId, ISet<string> services) : base(MessageHeader, route, messageId) {
			Services = services;
		}
		public WorkerConnect() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			for(int i = index; i< msg.FrameCount; i++) {
				this.Services.Add(msg[i].Buffer.ToUtf8String());
			}
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			foreach(var item in this.Services) {
				msg.AppendUtf8String(item);
			}
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			while(line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.Services.Add(text);
			}
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			foreach(var item in Services) {
				writer.Space().Append(item);
			}
		}
	}
}