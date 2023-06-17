using Albatross.Messaging.Messages;
using NetMQ;
using System.Linq;

namespace Albatross.Messaging.ReqRep.Messages {
	public record class WorkerResponse : Message, IMessage {
		public static string MessageHeader => "worker-response";
		public static IMessage Accept(string route, ulong messageId, NetMQMessage frames) {
			var payload = new byte[0];
			if (frames.Any()) {
				payload = frames.Pop().Buffer;
			}
			return new WorkerResponse(route, messageId, payload);
		}

		public byte[] Payload { get; init; }

		public WorkerResponse(string route, ulong messageId, byte[] payload) : base(MessageHeader, route, messageId) {
			Payload = payload;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.Append(Payload);
			return msg;
		}
	}
}