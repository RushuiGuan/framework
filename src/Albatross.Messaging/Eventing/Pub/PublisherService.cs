using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.Eventing.Pub {
	public class PublisherService : IRouterServerService {
		private SubscriptionManagement subscriberManagement = new SubscriptionManagement();
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
				case SubscriptionRequest req:
					if (!string.IsNullOrEmpty(msg.Route)) {
						if (req.On) {
							logger.LogInformation("subscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							subscriberManagement.Add(req.Pattern, req.Route);
						} else {
							logger.LogInformation("unsubscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							subscriberManagement.Remove(req.Pattern, new Subscriber(req.Route));
						}
						messagingService.Transmit(new SubscriptionReply(req.Route, req.Id, req.On, req.Pattern));
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
			if (msg is PubEvent pub) {
				foreach(var sub in subscriberManagement.Subscriptions) {
					if (sub.Match(pub.Topic)) {
						foreach(var subscriber in sub.Subscribers) {
							messagingService.Transmit(new Event(subscriber.Route, counter.NextId(), pub.Topic, sub.Pattern, pub.Payload));
						}
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
