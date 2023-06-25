using Albatross.Messaging.Messages;
using NetMQ;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class ClientRequest : Message, IMessage {
		public static string MessageHeader => "client-req";
		public byte[] Payload { get; init; }
		public string? Service { get; init; }


		public ClientRequest(string route, uint messageId, string? service, byte[] payload) : base(MessageHeader, route, messageId) {
			Payload = payload;
			Service = service;
		}
	}
}