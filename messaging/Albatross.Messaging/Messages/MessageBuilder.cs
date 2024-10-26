using NetMQ;

namespace Albatross.Messaging.Messages {
	public interface IMessageBuilder {
		string Header { get; }
		IMessage Build(NetMQMessage frames);
		IMessage Build(string line, int offset);
	}

	public class MessageBuilder<T> : IMessageBuilder where T : IMessage, new() {
		public string Header => T.MessageHeader;
		public IMessage Build(NetMQMessage frames) {
			var t = new T();
			t.ReadFromFrames(frames);
			return t;
		}
		public IMessage Build(string line, int offset) {
			var t = new T();
			t.ReadFromText(line, ref offset);
			return t;
		}
	}
}