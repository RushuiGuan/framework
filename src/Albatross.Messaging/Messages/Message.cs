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

		public virtual int StartingFrameIndex => HasRoute ? 4 : 3;
		public bool HasRoute => !string.IsNullOrEmpty(this.Route);


		public Message(string header, string route, ulong id) {
			this.Header = header;
			this.Route = route;
			this.Id = id;
		}
		public Message() { }


		public virtual void ReadFromFrames(NetMQMessage msg) {
			var index = 0;
			if (msg.HasRoute()) {
				this.Route = msg[index++].Buffer.ToUtf8String();
			} else {
				this.Route = string.Empty;
			}
			index++;
			this.Header = msg[index++].Buffer.ToUtf8String();
			this.Id = BitConverter.ToUInt64(msg[index++].Buffer);
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