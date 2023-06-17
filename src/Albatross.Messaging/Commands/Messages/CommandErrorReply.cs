using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandErrorReply : Message, IMessage {
		public static string MessageHeader => "cmd-err";
		public static IMessage Accept(string endpoint, ulong id, NetMQMessage frames) {
			return new CommandErrorReply(endpoint, id, frames.PopUtf8String(), frames.PopUtf8String());
		}
		public string ClassName { get; init; }
		public string Message { get; init; }

		public CommandErrorReply(string route, ulong id, string className, string message) : base(MessageHeader, route, id) {
			ClassName = className;
			Message = message;
		}

		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendUtf8String(ClassName);
			msg.AppendUtf8String(Message);
			return msg;
		}
	}
}
