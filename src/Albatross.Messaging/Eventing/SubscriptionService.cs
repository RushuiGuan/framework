using Albatross.Collections;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing {
	public class SubscriptionService : IDealerClientService {
		private AtomicCounter<ulong> counter = new AtomicCounter<ulong>();

		object sync = new object();
		Dictionary<string, ISet<ISubscriber>> subscriptions = new Dictionary<string, ISet<ISubscriber>>();
		Dictionary<ulong, SubscriberCallback> callbacks = new Dictionary<ulong, SubscriberCallback>();
		private readonly ILogger<SubscriptionService> logger;

		public SubscriptionService(ILogger<SubscriptionService> logger) {
			this.logger = logger;
		}

		public bool HasCustomTransmitObject => false;
		public bool CanReceive => true;
		public bool NeedTimer => false;

		public void Init(IMessagingService dealerClient) { }
		public bool ProcessReceivedMsg(IMessagingService dealerClient, IMessage msg) {
			switch (msg) {
				case SubscriptionReply sub_reply:
					lock (sync) {
						if (!sub_reply.On) {
							if (subscriptions.TryGetValue(sub_reply.Topic, out var set)) {
								subscriptions.Remove(sub_reply.Topic);
							}
						} else {
							if (callbacks.TryGetValue(sub_reply.Id, out var callback)) {
								callbacks.Remove(sub_reply.Id);
								var set = this.subscriptions.GetOrAdd(sub_reply.Topic, () => new HashSet<ISubscriber>());
								set.Add(callback.Subscriber);
								callback.SetResult();
							}
						}
					}
					break;
				case Event eve:
					dealerClient.Ack(eve.Route, eve.Id);
					ISet<ISubscriber>? subscribers = null;
					lock (sync) {
						subscriptions.TryGetValue(eve.Topic, out subscribers);
					}
					if (subscribers != null) {
						foreach (var item in subscribers) {
							// this kick of a different thread.  therefore don't need a try catch block
							_ = item.DataReceived(eve.Topic, eve.Payload);
						}
					}
					break;
				default:
					return false;
			}
			return true;
		}
		public bool ProcessTransmitQueue(IMessagingService dealerClient, object _) => false;
		public void ProcessTimerElapsed(DealerClient dealerClient) { }

		public Task<Subscription> Subscribe(DealerClient dealerClient, ISubscriber subscriber, string topic) {
			lock (sync) {
					Subscription result = new Subscription(topic, subscriber, true);
				if (subscriptions.TryGetValue(topic, out var subscribers)) {
					subscribers.Add(subscriber);
					return Task.FromResult(result);
				} else {
					var id = counter.NextId();
					var callback = new SubscriberCallback(id, result);
					callbacks.TryAdd(id, callback);
					dealerClient.SubmitToQueue(new SubscriptionRequest(string.Empty, id, true, newTopics));
					return callback.Task;
				}
			}
		}
		public Subscription Unsubscribe(DealerClient dealerClient, ISubscriber subscriber, params string[] topics) {
			lock (sync) {
				var currentTopics = new HashSet<string>();
				var topics_to_unsubscribe = new HashSet<string>();
				foreach (var topic in topics) {
					if (subscriptions.TryGetValue(topic, out var subscribers)) {
						subscribers.Remove(subscriber);
						if (subscriptions.Count == 0) {
							topics_to_unsubscribe.Add(topic);
						}
					}
				}
				Subscription result = new Subscription(topics, subscriber, false);
				if (topics_to_unsubscribe.Count > 0) {
					var id = counter.NextId();
					var callback = new SubscriberCallback(id, result);
					callbacks.TryAdd(id, callback);
					dealerClient.SubmitToQueue(new SubscriptionRequest(string.Empty, id, false, topics));
				}
				return result;
			}
		}
	}
}
