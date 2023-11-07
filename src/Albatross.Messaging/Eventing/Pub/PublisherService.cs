using Albatross.Messaging.EventSource;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;

namespace Albatross.Messaging.Eventing.Pub {
	public interface IPublisherService : IRouterServerService { }
	public class PublisherService : IPublisherService {
		private SubscriptionManagement subscriberManagement = new SubscriptionManagement();
		private readonly ILogger<PublisherService> logger;

		public bool CanReceive => true;
		public bool HasCustomTransmitObject => true;
		/// <summary>
		///  need to periodically save all subscription
		/// </summary>
		public bool NeedTimer => true;

		public PublisherService(ILogger<PublisherService> logger) {
			this.logger = logger;
		}

		public bool ProcessReceivedMsg(IMessagingService messagingService, IMessage msg) {
			switch (msg) {
				case Connect connect:
					// need to replay messages here
					return false;
				case UnsubscribeAllRequest unsubAll:
					logger.LogInformation("unsubscribing all for {route}", unsubAll.Route);
					subscriberManagement.UnsubscribeAll(unsubAll.Route);
					messagingService.Transmit(new ServerAck(unsubAll.Route, unsubAll.Id));
					return true;
				case SubscriptionRequest req:
					if (!string.IsNullOrEmpty(msg.Route)) {
						if (req.On) {
							logger.LogInformation("subscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							subscriberManagement.Add(req.Pattern, req.Route);
						} else {
							logger.LogInformation("unsubscribing route {route} using pattern: {pattern}", req.Route, req.Pattern);
							subscriberManagement.Remove(req.Pattern, req.Route);
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
				foreach (var sub in subscriberManagement.Subscriptions) {
					if (sub.Match(pub.Topic)) {
						foreach (var subscriber in sub.Subscribers) {
							var eve = new Event(subscriber, messagingService.Counter.NextId(), pub.Topic, sub.Pattern, pub.Payload);
							// if a subscriber is no longer connected, record the msg but don't send it so that we don't overrun the buffer
							if(messagingService.GetClientState(subscriber) == ClientState.Alive) { 
								messagingService.Transmit(eve);
							} else {
								messagingService.DataLogger.WriteLogEntry(new LogEntry(EntryType.Out, eve));
							}
						}
					}
				}
				return true;
			} else {
				return false;
			}
		}
		public void ProcessTimerElapsed(IMessagingService routerServer, ulong count) {
			if (subscriberManagement.ShouldSave) {
				logger.LogInformation("saving publisher subscriptions");
				this.subscriberManagement.Save(routerServer.DataLogger, routerServer.Counter);
			}
		}
	}
}
