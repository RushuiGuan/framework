using Albatross.Messaging.DataLogging;
using Albatross.Reflection;
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
		public IMessage Create(bool hasRoute, NetMQMessage frames, IDataLogWriter logWriter) {
			var route = string.Empty;
			if (hasRoute) {
				route = frames.PopUtf8String();
			}
			frames.Pop();
			var header = frames.PopUtf8String();
			var messageId = frames.PopULong();

			logWriter.Incoming(route, header, messageId, frames);

			IMessage msg;
			if (this.builders.TryGetValue(header, out var builder)) {
				msg = builder.Build(route, messageId, frames);
			} else {
				msg =new UnknownMsg(route, header, messageId, frames);
			}
			return msg;
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
		IMessage Create(bool hasRoute, NetMQMessage frames, IDataLogWriter logWriter);
		IMessage Create(DataLog replay);
	}
}
