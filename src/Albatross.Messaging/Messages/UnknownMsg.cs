using NetMQ;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Albatross.Messaging.Messages {
	public record class UnknownMsg : Message, IMessage {
		public static string MessageHeader => "unknown";
		public UnknownMsg() { }
		public List<byte[]> PayLoad { get; private set; } = new List<byte[]>();
	}
}