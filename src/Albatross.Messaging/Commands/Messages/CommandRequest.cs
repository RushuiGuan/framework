using Albatross.Text;
using CoenM.Encoding;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;
using System.IO;

namespace Albatross.Messaging.Commands.Messages {
	public record class CommandRequest : Message, IMessage {
		public static string MessageHeader => "cmd-req";
		public static IMessage Accept(string route, ulong id, NetMQMessage frames) {
			var commandType = frames.PopUtf8String();
			var fireAndForget = frames.PopBoolean();
			var payload = frames.Pop().Buffer;
			return new CommandRequest(route, id, commandType, fireAndForget, payload);
		}
		public string CommandType { get; init; }
		public bool FireAndForget { get; init; }
		public byte[] Payload { get; init; }

		public CommandRequest(string route, ulong id, string commandType, bool fireAndForget, byte[] payload) : base(MessageHeader, route, id) {
			CommandType = commandType;
			FireAndForget = fireAndForget;
			Payload = payload;
		}


		public override NetMQMessage Create() {
			var msg = base.Create();
			msg.AppendUtf8String(CommandType);
			msg.AppendBoolean(FireAndForget);
			msg.Append(Payload);
			return msg;
		}
	}
}