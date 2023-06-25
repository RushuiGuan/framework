using Albatross.Messaging.Messages;
using NetMQ;
using System;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class ClientRequest : Message, IMessage {
		public static string MessageHeader => "client-req";
		public byte[] Payload { get; private set; } = Array.Empty<byte>();
		public string? Service { get; private set; }


		public ClientRequest(string route, uint messageId, string? service, byte[] payload) : base(MessageHeader, route, messageId) {
			Payload = payload;
			Service = service;
		}
		public ClientRequest() { }
	}
}