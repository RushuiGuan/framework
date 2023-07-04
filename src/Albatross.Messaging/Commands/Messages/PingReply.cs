using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using NetMQ;

namespace Albatross.Messaging.Commands.Messages {
	public record class PingReply : Message, IMessage {
		public static string MessageHeader { get => "ping-rep"; }

		public PingReply(string route, ulong id) : base(MessageHeader, route, id) { }
		public PingReply() { }
	}
}
