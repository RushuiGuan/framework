using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandExecuted2 : CommandMessage {
		public static string MessageHeader => "cmd-exec2";
		public CommandExecuted2(string route, ulong id, string commandType) : base(MessageHeader, route, id, commandType) {
		}
		public CommandExecuted2() { }
	}
}