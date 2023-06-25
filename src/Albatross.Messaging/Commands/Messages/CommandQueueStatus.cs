using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandQueueStatus : Message, IMessage {
		public static string MessageHeader => "queue-status";

		public CommandQueueStatus(string route, ulong id) : base(MessageHeader, route, id) { }
		public CommandQueueStatus() { }
	}
}
