using Albatross.Messaging.Messages;
using NetMQ;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class ClientRequest : Message, IMessage {
		public static string MessageHeader => "c:req";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) {
			var service = string.Empty;
			var payload = new byte[0];
			var id = frames.PopUInt();
			if (frames.Any()) {
				service = frames.PopUtf8String();
				if (frames.Any()) {
					payload = frames.Pop().Buffer;
				}
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
			msg.Append(Payload);
			return msg;
		}
	}
}