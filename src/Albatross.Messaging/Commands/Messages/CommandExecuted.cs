using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandExecuted : Message, IMessage {
		public static string MessageHeader => "cmd-exec";
		public CommandExecuted(string route, ulong id) : base(MessageHeader, route, id) {
		}
		public CommandExecuted() { }
	}
}