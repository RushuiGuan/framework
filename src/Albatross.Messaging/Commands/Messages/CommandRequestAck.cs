using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandRequestAck : Message, IMessage {
		public static string MessageHeader => "cmd-req-ack";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) => new CommandRequestAck(route, id);
		public CommandRequestAck(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}