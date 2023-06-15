using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class BrokerRequest : Message, IMessage {
		public static string MessageHeader => "b:req";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) {
			var client = frames.PopUtf8String();
			byte[] payload;
			if (frames.Any()) {
				payload = frames.Pop().Buffer;
			} else {
				payload = new byte[0];
			}
			return new BrokerRequest(route, messageId, client, payload);
		}

		public string Client { get; init; }
		public byte[] Payload { get; init; }


		public BrokerRequest(string route, ulong messageId, string client, byte[] payload) : base(MessageHeader, route, messageId) {
			Client = client;
			Payload = payload;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendUtf8String(Client);
			if (Payload.Length > 0) {
				msg.Append(Payload);
			}
			return msg;
		}
	}
}