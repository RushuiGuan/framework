using Albatross.Messaging.Commands.Messages;
using Albatross.Messaging.EventSource;
using Albatross.Messaging.PubSub.Messages;
using Albatross.Messaging.ReqRep.Messages;
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
		void Add(IMessageBuilder builder) {
			this.builders.Add(builder.Header, builder);
		}

		public MessageFactory() {
			Add(new MessageBuilder<CommandErrorReply>());
			Add(new MessageBuilder<CommandReply>());
			Add(new MessageBuilder<CommandRequest>());
			Add(new MessageBuilder<CommandRequestAck>());
			Add(new MessageBuilder<CommandRequestError>());
			Add(new MessageBuilder<ClientAck>());
			Add(new MessageBuilder<Connect>());
			Add(new MessageBuilder<ConnectOk>());
			Add(new MessageBuilder<Heartbeat>());
			Add(new MessageBuilder<HeartbeatAck>());
			Add(new MessageBuilder<Reconnect>());
			Add(new MessageBuilder<Resume>());
			Add(new MessageBuilder<ServerAck>());
			Add(new MessageBuilder<UnknownMsg>());
			Add(new MessageBuilder<Event>());
			Add(new MessageBuilder<SubscriptionReply>());
			Add(new MessageBuilder<SubscriptionRequest>());
			Add(new MessageBuilder<UnsubscribeAllRequest>());
			Add(new MessageBuilder<BrokerAck>());
			Add(new MessageBuilder<BrokerRequest>());
			Add(new MessageBuilder<BrokerResponse>());
			Add(new MessageBuilder<ClientRequest>());
			Add(new MessageBuilder<NoAvailableWorker>());
			Add(new MessageBuilder<WorkerAck>());
			Add(new MessageBuilder<WorkerResponse>());
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