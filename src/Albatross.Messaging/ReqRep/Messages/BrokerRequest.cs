using Albatross.Messaging.Messages;
using NetMQ;
using System;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class BrokerRequest : Message, IMessage {
		public static string MessageHeader => "req";
		public string Client { get; init; } = string.Empty;
		public byte[] Payload { get; init; }= Array.Empty<byte>();


		public BrokerRequest(string route, ulong messageId, string client, byte[] payload) : base(MessageHeader, route, messageId) {
			Client = client;
			Payload = payload;
		}
		public BrokerRequest() { }
	}
}