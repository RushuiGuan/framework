using Albatross.Messaging.Messages;
using NetMQ;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class BrokerRequest : Message, IMessage {
		public static string MessageHeader => "req";
		public string Client { get; init; }
		public byte[] Payload { get; init; }


		public BrokerRequest(string route, ulong messageId, string client, byte[] payload) : base(MessageHeader, route, messageId) {
			Client = client;
			Payload = payload;
		}
	}
}