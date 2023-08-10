using Albatross.Collections;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing.Sub {
	public class SubscriptionService : IDealerClientService {
		private readonly object sync = new object();
		private readonly Dictionary<string, ISet<ISubscriber>> subscriptions = new Dictionary<string, ISet<ISubscriber>>();
		private readonly Dictionary<ulong, IMessageCallback> callbacks = new Dictionary<ulong, IMessageCallback>();
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
				case ServerAck ack:
					lock (sync) {
						if(callbacks.TryGetAndRemove(ack.Id, out var callback)) {
							callback.SetResult();
							return true;
						}
					}
					break;
				case SubscriptionReply sub_reply:
					lock (sync) {
						if (callbacks.TryGetAndRemove(sub_reply.Id, out var callback)) {
							if (sub_reply.On) {
								if (callback is SubscriptionCallback subCallback) {
									var set = subscriptions.GetOrAdd(sub_reply.Pattern, () => new HashSet<ISubscriber>());
									set.Add(subCallback.Subscriber);
								} else {
									logger.LogError("Sub callback {id} is of the wrong type: {type}", sub_reply.Id, callback.GetType().FullName);
								}
							} else {
								if (subscriptions.TryGetAndRemove(sub_reply.Pattern, out var set)) {
									logger.LogInformation("Sub pattern {pattern} has been removed from server", sub_reply.Pattern);
								}
							}
							callback.SetResult();
						}
					}
					return true;
				case Event eve:
					dealerClient.ClientAck(eve.Route, eve.Id);
					ISet<ISubscriber>? subscribers = null;
					lock (sync) {
						subscriptions.TryGetValue(eve.Pattern, out subscribers);
					}
					if (subscribers != null) {
						foreach (var item in subscribers) {
							// this kick of a different thread.  therefore don't need a try catch block
							Task.Run(async () => {
								try {
									await item.DataReceived(eve.Topic, eve.Payload);
								} catch (Exception err) {
									logger.LogError(err, "Error processing subscriber {name}", item.Name);
								}
							});
						}
					}
					return true;
			}
			return false;
		}
		public bool ProcessTransmitQueue(IMessagingService dealerClient, object _) => false;
		public void ProcessTimerElapsed(DealerClient dealerClient) { }

		/// <summary>
		/// thread safe call to subscribe to a pattern
		/// </summary>
		public Task Subscribe(DealerClient dealerClient, ISubscriber subscriber, string pattern) {
			lock (sync) {
				var id = dealerClient.Counter.NextId();
				var callback = new SubscriptionCallback(subscriber);
				callbacks.TryAdd(id, callback);
				dealerClient.SubmitToQueue(new SubscriptionRequest(string.Empty, id, true, pattern));
				return callback.Task;
			}
		}
		/// <summary>
		/// thread safe call to unsubscribe to a pattern
		/// </summary>
		public Task Unsubscribe(DealerClient dealerClient, ISubscriber subscriber, string pattern) {
			lock (sync) {
				var unsubscribeFromServer = false;
				logger.LogInformation("trying to unsubscribe {pattern} for {subscriber}", pattern, subscriber);
				if (subscriptions.TryGetValue(pattern, out var subscribers)) {
					subscribers.Remove(subscriber);
					if (subscribers.Count == 0) {
						unsubscribeFromServer = true;
					}
				} else {
					logger.LogInformation("subscription pattern not found");
				}
				if (unsubscribeFromServer) {
					var id = dealerClient.Counter.NextId();
					var callback = new MessageCallback();
					callbacks.TryAdd(id, callback);
					dealerClient.SubmitToQueue(new SubscriptionRequest(string.Empty, id, false, pattern));
					return callback.Task;
				} else {
					return Task.CompletedTask;
				}
			}
		}

		/// <summary>
		/// thread safe call to unsubscribe to a pattern
		/// </summary>
		public Task UnsubscribeAll(DealerClient dealerClient) {
			lock (sync) {
				logger.LogInformation("UnsubscribAll from {client}", dealerClient.Identity);
				subscriptions.Clear();
				var id = dealerClient.Counter.NextId();
				var callback = new MessageCallback();
				callbacks.TryAdd(id, callback);
				dealerClient.SubmitToQueue(new UnsubscribeAllRequest(string.Empty, id));
				return callback.Task;
			}
		}
	}
}
