using Albatross.Messaging.Configurations;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;

namespace Albatross.Messaging.Eventing.Pub {
	public class PublisherService : IRouterServerService {
		private SubscriptionManagement subscriberManagement = new SubscriptionManagement();
		private readonly RouterServerConfiguration config;
		private readonly ILogger<PublisherService> logger;
		private readonly AtomicCounter<ulong> counter = new AtomicCounter<ulong>();

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => true;
		public bool NeedTimer => true;

		public PublisherService(RouterServerConfiguration config, ILogger<PublisherService> logger) {
			this.config = config;
			this.logger = logger;
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) {
			switch (msg) {
				case SubscriptionRequest req:
					if (!string.IsNullOrEmpty(msg.Route)) {
						if (req.On) {
							logger.LogInformation("subscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							var sub = subscriberManagement.Add(req.Pattern, req.Route);
							sub.SetState(SubscriberState.Connected).SetHeartBeat();
						} else {
							logger.LogInformation("unsubscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							subscriberManagement.Remove(req.Pattern, req.Route);
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
							var eve = new Event(subscriber.Route, counter.NextId(), pub.Topic, sub.Pattern, pub.Payload);
							// if a subscriber is no longer connected, record the msg but don't send it so that we don't overrun the buffer
							if (subscriber.State == SubscriberState.Connected) {
								messagingService.Transmit(eve);
							} else {
								messagingService.DataLogger.Outgoing(eve, eve.Create());
							}
						}
					}
				}
				return true;
			} else {
				return false;
			}
		}
		public void ProcessTimerElapsed(IMessagingService routerServer) {
			foreach(var sub in subscriberManagement.Subscribers) {
				sub.CheckState(TimeSpan.FromMilliseconds(config.ActualTimerInterval * 1.2));
			}
		}
	}
}
