using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandQueueStatusReply : Message, IMessage {
		public static string MessageHeader => "queue-status-reply";
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) {
			return new CommandQueueStatusReply(endpoint, id, frames.Pop().Buffer);
		}
		public byte[] Payload { get; init; }

		public CommandQueueStatusReply(string route, ulong id, byte[] payload) : base(MessageHeader, route, id) {
			Payload = payload;
		}


		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.Append(Payload);
			return msg;
		}
	}
}
