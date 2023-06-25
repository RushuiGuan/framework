using Albatross.Text;
using NetMQ;
using System;
using System.IO;
using System.Linq;

namespace Albatross.Messaging.Messages {
	public abstract record class Message {
		public const char LogDelimiter = ' ';
		public string Route { get; private set; } = string.Empty;
		public string Header { get; private set; } = string.Empty;
		public ulong Id { get; private set; }

		public int StartingFrameIndex => 3;
		public virtual int Size => 4;
		public bool HasRoute => !string.IsNullOrEmpty(this.Route);


		public Message(string route, string header, ulong id) {
			this.Route = route;
			this.Header = header;
			this.Id = id;
		}
		public Message() { }

		
		public virtual void ReadFromFrames(NetMQMessage msg) {
			if (msg.HasRoute()) {
				this.Route = msg[0].Buffer.ToUtf8String();
			} else {
				this.Route = string.Empty;
			}
			this.Header = msg[2].Buffer.ToUtf8String();
			this.Id = BitConverter.ToUInt64(msg[3].Buffer);
		}
		public virtual void WriteToFrames(NetMQMessage msg) {
			msg.Clear();
			if (HasRoute) {
				msg.Append(new NetMQFrame(this.Route.ToUtf8Bytes()));
			}
			msg.AppendEmptyFrame();
			msg.Append(Header.ToUtf8Bytes());
			msg.Append(BitConverter.GetBytes(this.Id));
		}
		public virtual void ReadFromText(string line, ref int offset) {
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.Header = text;
				if (line.TryGetText(LogDelimiter, ref offset, out text)) {
					this.Route = text;
					if (line.TryGetText(LogDelimiter, ref offset, out text)) {
						if (ulong.TryParse(text, out var value)) {
							this.Id = value;
							return;
						}
					}
				}
			}
			throw new InvalidMsgLogException(line);
		}

		public virtual void WriteToText(TextWriter writer) {
			writer.Append(Header).Space().Append(Route).Space().Append(Id);
		}

		protected string Encode(byte[] bytes) => CoenM.Encoding.Z85Extended.Encode(bytes);
		protected byte[] Decode(string text) => CoenM.Encoding.Z85Extended.Decode(text);
	}
}