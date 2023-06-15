using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandExecuted : Message, IMessage {
		public static string MessageHeader => "cmd-exec";
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) {
			return new CommandExecuted(endpoint, id);
		}
		public CommandExecuted(string route, ulong id) : base(MessageHeader, route, id) {
		}
	}
}