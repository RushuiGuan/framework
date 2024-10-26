using Albatross.Messaging.EventSource;
using Albatross.Reflection;
using Microsoft.Extensions.Logging;
using NetMQ;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Albatross.Messaging.Messages {
	public class MessageFactory : IMessageFactory {
		Dictionary<string, IMessageBuilder> builders = new Dictionary<string, IMessageBuilder>();

		public MessageFactory() {
			var types = Assembly.GetExecutingAssembly().GetConcreteClasses<IMessage>();
			foreach (var type in types) {
				Type genericType = typeof(MessageBuilder<>).MakeGenericType(type) ?? throw new NotSupportedException();
				var builder = (IMessageBuilder)(Activator.CreateInstance(genericType) ?? throw new NotSupportedException());
				this.builders.Add(builder.Header, builder);
			}
		}
		public IMessage Create(NetMQMessage frames) {
			var header = frames.PeekMessageHeader();
			IMessage msg;
			if (this.builders.TryGetValue(header, out var builder)) {
				msg = builder.Build(frames);
			} else {
				msg = new UnknownMsg();
				msg.ReadFromFrames(frames);
			}
			return msg;
		}

		public IMessage Create(string line, int offset) {
			var header = line.PeekMessageHeader(offset);
			if (this.builders.TryGetValue(header, out var builder)) {
				return builder.Build(line, offset);
			} else {
				var msg = new UnknownMsg();
				msg.ReadFromText(line, ref offset);
				return msg;
			}
		}
	}

	public interface IMessageFactory {
		IMessage Create(NetMQMessage frames);
		IMessage Create(string line, int offset);
	}
}