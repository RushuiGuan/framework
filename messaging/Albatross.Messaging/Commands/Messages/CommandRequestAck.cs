using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandRequestAck : Message, IMessage {
		public static string MessageHeader => "cmd-req-ack";
		public CommandRequestAck(string route, ulong id) : base(MessageHeader, route, id) { }
		public CommandRequestAck() { }
	}
}