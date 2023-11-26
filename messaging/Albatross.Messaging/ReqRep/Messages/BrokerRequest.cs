﻿using Albatross.Text;
using Albatross.Messaging.Messages;
using NetMQ;
using System;
using System.IO;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class BrokerRequest : Message, IMessage {
		public static string MessageHeader => "broker-req";
		public string ClientId { get; private set; } = string.Empty;
		public ulong RequestId { get; private set; }
		public byte[] Payload { get; private set; }= Array.Empty<byte>();


		public BrokerRequest(string route, ulong messageId, string clientId, ulong requestId, byte[] payload) : base(MessageHeader, route, messageId) {
			this.ClientId = clientId;
			this.RequestId = requestId;
			this.Payload = payload;
		}
		public BrokerRequest() { }

		public override void ReadFromFrames(NetMQMessage msg) {
			base.ReadFromFrames(msg);
			var index = base.StartingFrameIndex;
			this.ClientId = msg[index++].Buffer.ToUtf8String();
			this.RequestId = msg[index++].Buffer.ToULong();
			this.Payload = msg[index++].Buffer;
		}
		public override void WriteToFrames(NetMQMessage msg) {
			base.WriteToFrames(msg);
			msg.AppendUtf8String(this.ClientId);
			msg.AppendULong(this.RequestId);
			msg.Append(this.Payload);
		}
		public override void ReadFromText(string line, ref int offset) {
			base.ReadFromText(line, ref offset);
			if (line.TryGetText(LogDelimiter, ref offset, out var text)) {
				this.ClientId = text;
				if (line.TryGetText(LogDelimiter, ref offset, out text) && ulong.TryParse(text, out var ulongValue)) {
					this.RequestId = ulongValue;
					if (line.TryGetText(LogDelimiter, ref offset, out text)) {
						this.Payload = Decode(text);
						return;
					}
				}
			}
			throw new InvalidMsgLogException(line);
		}
		public override void WriteToText(TextWriter writer) {
			base.WriteToText(writer);
			writer.Space().Append(ClientId)
				.Space().Append(RequestId)
				.Space().Append(Encode(Payload));
		}
	}
}