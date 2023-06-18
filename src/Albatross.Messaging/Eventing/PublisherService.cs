using Albatross.Collections;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Messaging.Eventing {
	public class PublisherService : IRouterServerService {
		Dictionary<string, ISet<string>> subscriptions = new Dictionary<string, ISet<string>>();
		private readonly ILogger<PublisherService> logger;
		private readonly AtomicCounter<ulong> counter = new AtomicCounter<ulong>();

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => true;
		public bool NeedTimer => true;

		public PublisherService(ILogger<PublisherService> logger) {
			this.logger = logger;
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) {
			switch (msg) {
				case SubscriptionRequest sub:
					if (!string.IsNullOrEmpty(msg.Route)) {
						foreach (var topic in sub.Topic) {
							var set = this.subscriptions.GetOrAdd(topic, () => new HashSet<string>());
							if (sub.On) {
								set.Add(msg.Route);
							} else {
								set.Remove(msg.Route);
							}
						}
						messagingService.Transmit(new SubscriptionReply(sub.Route, sub.Id, sub.On, sub.Topic));
					} else {
						logger.LogError("receive a subscription msg without the subcriber identity: {msg}", msg);
					}
					break;
				default:
					return false;
			}
			return true;
		}
		public bool ProcessTransmitQueue(IMessagingService messagingService, object msg) {
			if(msg is PubEvent pub) {
				if (subscriptions.TryGetValue(pub.Topic, out var subscribers) && subscribers.Count() > 0) {
					foreach (var sub in subscribers) {
						messagingService.Transmit(new Event(sub, counter.NextId(), pub.Topic, pub.Payload));
					}
				}
				return true;
			} else {
				return false;
			}
		}
		public void ProcessTimerElapsed(IMessagingService routerServer) {
		}
	}
}
