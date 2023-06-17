using NetMQ;
using System;

namespace Albatross.Messaging.Messages {
	public interface IMessage {
		virtual static string MessageHeader { get => throw new NotImplementedException(); }
		virtual static IMessage Accept(string route, ulong messageId, NetMQMessage frames) => throw new NotImplementedException();

		string? Route { get; }
		string Header { get; }
		ulong Id { get; }
		NetMQMessage Create();
	}

	public abstract record class Message {
		public string Route { get; init; } = string.Empty;
		public string Header { get; init; }
		public ulong Id { get; init; }

		public Message(string header, string route, ulong id) {
			this.Header = header;
			this.Route = route;
			this.Id = id;
		}
		public virtual NetMQMessage Create() {
			var msg = new NetMQMessage(5);
			if (!string.IsNullOrEmpty(Route)) {
				msg.AppendUtf8String(Route);
			}
			msg.AppendEmptyFrame();
			msg.AppendUtf8String(Header);
			msg.AppendULong(Id);
			return msg;
		}

		public override string ToString() {
			if (string.IsNullOrEmpty(Route)) {
				return $"route {Header} {Id}";
			} else {
				return $"{Route} {Header} {Id}";
			}
		}
	}
}