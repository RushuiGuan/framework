﻿using Albatross.Text;
using Albatross.Messaging.Messages;
using NetMQ;
using System.IO;
using System;

namespace Albatross.Messaging.Eventing.Messages {
	public record class SubscriptionRequest : Message, IMessage {
		public static string MessageHeader => "sub";

		public SubscriptionRequest(string route, ulong id, bool on, string pattern) : base(MessageHeader, route, id) {
			this.On = on;
			this.Pattern = pattern;
		}

		public bool On { get; private set; }
		public string Pattern { get; private set; }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = this.StartingFrameIndex;
			this.On = msg[index++].Buffer.ToBoolean();
			this.Pattern = msg[index++].Buffer.ToUtf8String();
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.Append(BitConverter.GetBytes(this.On));
			msg.Append(this.Pattern.ToUtf8Bytes());
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text) && bool.TryParse(text, out var booleanValue)) {
				this.On = booleanValue;
				if (line.TryGetText(LogDelimiter, ref offset, out text)) {
					this.Pattern = text;
					return;
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(this.On)
				.Space().Append(this.Pattern);
		}
	}
}