using Albatross.Messaging.Messages;
using NetMQ;

namespace Albatross.Messaging.Messages {
	public interface IMessageBuilder {
		string Header { get; }
		IMessage Build(string route, ulong messageId, NetMQMessage frames);
	}

	public class MessageBuilder<T> : IMessageBuilder where T : IMessage {
		public string Header => T.MessageHeader;
		public IMessage Build(string route, ulong messageId, NetMQMessage frames)
			=> T.Accept(route, messageId, frames);
	}
}
