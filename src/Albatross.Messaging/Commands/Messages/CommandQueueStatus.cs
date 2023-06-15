using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandQueueStatus : Message, IMessage {
		public static string MessageHeader => "queue-status";

		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) {
			return new CommandQueueStatus(endpoint, id);
		}
		public CommandQueueStatus(string route, ulong id) : base(MessageHeader, route, id) { }
	}
}
