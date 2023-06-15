using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandReply : Message, IMessage {
		public static string MessageHeader => "cmd-rep";
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) {
			var payload = frames.Pop().Buffer;
			return new CommandReply(endpoint, id, payload);
		}
		public byte[] Payload { get; init; }

		public CommandReply(string route, ulong id, byte[] payload) : base(MessageHeader, route, id) {
			Payload = payload;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.Append(Payload);
			return msg;
		}
	}
}