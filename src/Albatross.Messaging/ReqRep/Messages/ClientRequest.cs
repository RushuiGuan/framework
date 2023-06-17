using Albatross.Messaging.Messages;
using NetMQ;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class ClientRequest : Message, IMessage {
		public static string MessageHeader => "client-req";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) {
			var payload = new byte[0];
			var id = frames.PopUInt();
			var service = frames.PopUtf8String();
			if (frames.Any()) {
				payload = frames.Pop().Buffer;
			}
			return new ClientRequest(route, id, service, payload);
		}

		public byte[] Payload { get; init; }
		public string? Service { get; init; }


		public ClientRequest(string route, uint messageId, string? service, byte[] payload) : base(MessageHeader, route, messageId) {
			Payload = payload;
			Service = service;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendUtf8String(Service);
			if (Payload.Any()) {
				msg.Append(Payload);
			}
			return msg;
		}
	}
}