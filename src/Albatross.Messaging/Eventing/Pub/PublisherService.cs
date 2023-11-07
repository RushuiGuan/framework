using Albatross.Messaging.EventSource;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.Eventing.Pub {
	public interface IPublisherService : IRouterServerService { }
	public class PublisherService : IPublisherService {
		private readonly ILogger<PublisherService> logger;
		private readonly ISubscriptionManagement subscriptionManagement;

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => true;
		public bool NeedTimer => false;

		public PublisherService(ILogger<PublisherService> logger, ISubscriptionManagement subscriptionManagement) {
			this.logger = logger;
			this.subscriptionManagement = subscriptionManagement;
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) {
			switch (msg) {
				case Connect connect:
					// need to replay messages here
					return false;
				case UnsubscribeAllRequest unsubAll:
					logger.LogInformation("unsubscribing all for {route}", unsubAll.Route);
					subscriptionManagement.UnsubscribeAll(unsubAll.Route);
					messagingService.Transmit(new ServerAck(unsubAll.Route, unsubAll.Id));
					return true;
				case SubscriptionRequest req:
					if (!string.IsNullOrEmpty(msg.Route)) {
						if (req.On) {
							logger.LogInformation("subscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							subscriptionManagement.Add(req.Pattern, req.Route);
						} else {
							logger.LogInformation("unsubscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							subscriptionManagement.Remove(req.Pattern, req.Route);
						}
						messagingService.Transmit(new SubscriptionReply(req.Route, req.Id, req.On, req.Pattern));
					} else {
						logger.LogError("receive a subscription msg without the subcriber identity: {msg}", msg);
					}
					return true;
			}
			return false;
		}
		public bool ProcessQueue(IMessagingService messagingService, object msg) {
			if (msg is PubEvent pub) {
				foreach (var sub in subscriptionManagement.Subscriptions) {
					if (sub.Match(pub.Topic)) {
						foreach (var subscriber in sub.Subscribers) {
							var eve = new Event(subscriber, messagingService.Counter.NextId(), pub.Topic, sub.Pattern, pub.Payload);
							// if a subscriber is no longer connected, record the msg but don't send it so that we don't overrun the buffer
							if (messagingService.GetClientState(subscriber) == ClientState.Alive) {
								messagingService.Transmit(eve);
							} else {
								messagingService.DataLogger.WriteLogEntry(new EventEntry(EntryType.Out, eve));
							}
						}
					}
				}
				return true;
			} else {
				return false;
			}
		}
		public void ProcessTimerElapsed(IMessagingService routerServer, ulong count) { }
	}
}
