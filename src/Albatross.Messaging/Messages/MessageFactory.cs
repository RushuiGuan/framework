using Albatross.Messaging.DataLogging;
using Albatross.Reflection;
using NetMQ;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Albatross.Messaging.Messages {
	public class MessageFactory : IMessageFactory {
		private readonly IDataLogWriter logWriter;
		Dictionary<string, IMessageBuilder> builders = new Dictionary<string, IMessageBuilder>();

		public MessageFactory(IDataLogWriter logWriter) {
			var types = Assembly.GetExecutingAssembly().GetConcreteClasses<IMessage>();
			foreach (var type in types) {
				Type genericType = typeof(MessageBuilder<>).MakeGenericType(type) ?? throw new NotSupportedException();
				var builder = (IMessageBuilder)(Activator.CreateInstance(genericType) ?? throw new NotSupportedException());
				this.builders.Add(builder.Header, builder);
			}

			this.logWriter = logWriter;
		}
		public IMessage Create(bool hasRoute, NetMQMessage frames) {
			var route = string.Empty;
			if (hasRoute) {
				route = frames.PopUtf8String();
			}
			frames.Pop();
			var header = frames.PopUtf8String();
			var messageId = frames.PopULong();

			this.logWriter.Incoming(route, header, messageId, frames);

			if (this.builders.TryGetValue(header, out var builder)) {
				return builder.Build(route, messageId, frames);
			} else {
				return new UnknownMsg(route, header, messageId, frames);
			}
		}

		public IMessage Create(DataLog replay) {
			var frames = new NetMQMessage();
			foreach(var bytes in replay.Payload) {
				frames.Append(bytes);
			}
			if (this.builders.TryGetValue(replay.Header, out var builder)) {
				return builder.Build(replay.Route, replay.MessageId, frames);
			} else {
				return new UnknownMsg(replay.Header, replay.Route, replay.MessageId, frames);
			}
		}
	}

	public interface IMessageFactory {
		IMessage Create(bool hasRoute, NetMQMessage frames);
		IMessage Create(DataLog replay);
	}
}
