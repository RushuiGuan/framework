using Albatross.Collections;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing.Sub {
	public class SubscriptionService : IDealerClientService {
		private readonly object sync = new object();
		private readonly AtomicCounter<ulong> counter = new AtomicCounter<ulong>();
		private readonly Dictionary<string, ISet<ISubscriber>> subscriptions = new Dictionary<string, ISet<ISubscriber>>();
		private readonly Dictionary<ulong, SubscriptionCallback> callbacks = new Dictionary<ulong, SubscriptionCallback>();
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
						if (callbacks.TryGetAndRemove(sub_reply.Id, out var callback)) {
							if (sub_reply.On) {
								var set = subscriptions.GetOrAdd(sub_reply.Pattern, () => new HashSet<ISubscriber>());
								set.Add(callback.Subscriber);
							} else {
								if (subscriptions.TryGetAndRemove(sub_reply.Pattern, out var set)) {
									logger.LogInformation("Sub pattern {pattern} has been removed from server", sub_reply.Pattern);
								}
							}
							callback.SetResult();
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

		/// <summary>
		/// thread safe call to subscribe to a pattern
		/// </summary>
		public Task<Subscription> Subscribe(DealerClient dealerClient, ISubscriber subscriber, string pattern) {
			lock (sync) {
				Subscription result = new Subscription(pattern, subscriber, true);
				if (subscriptions.TryGetValue(pattern, out var subscribers)) {
					subscribers.Add(subscriber);
					return Task.FromResult(result);
				} else {
					var id = counter.NextId();
					var callback = new SubscriptionCallback(id, result);
					callbacks.TryAdd(id, callback);
					dealerClient.SubmitToQueue(new SubscriptionRequest(string.Empty, id, true, pattern));
					return callback.Task;
				}
			}
		}
		/// <summary>
		/// thread safe call to unsubscribe to a pattern
		/// </summary>
		public Subscription Unsubscribe(DealerClient dealerClient, ISubscriber subscriber, string pattern) {
			lock (sync) {
				var unsubscribeFromServer = false;
				if (subscriptions.TryGetValue(pattern, out var subscribers)) {
					subscribers.Remove(subscriber);
					if (subscriptions.Count == 0) {
						unsubscribeFromServer = true;
					}
				}
				Subscription result = new Subscription(pattern, subscriber, false);
				if (unsubscribeFromServer) {
					var id = counter.NextId();
					var callback = new SubscriptionCallback(id, result);
					callbacks.TryAdd(id, callback);
					dealerClient.SubmitToQueue(new SubscriptionRequest(string.Empty, id, false, pattern));
				}
				return result;
			}
		}
	}
}
