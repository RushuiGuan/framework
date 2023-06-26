using Albatross.Collections;
using Albatross.Messaging.DataLogging;
using Albatross.Messaging.Eventing.Messages;
using Albatross.Messaging.Messages;
using Albatross.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Messaging.Eventing.Pub {
	public class SubscriptionManagement {
		public const int SubscriptionPersistentIntervalInMinutes = 30;

		ISet<Subscription> subscriptions = new HashSet<Subscription>();
		public IEnumerable<Subscription> Subscriptions => this.subscriptions;

		public void Add(string pattern, string route) {
			var subscription = subscriptions.GetOrAdd(item => item.Pattern == pattern, () => new Subscription(pattern));
			subscription.Subscribers.Add(route);
		}
		public void Remove(string pattern, string route) {
			var item = subscriptions.Where(args => args.Pattern == pattern).FirstOrDefault();
			if (item != null) {
				item.Subscribers.Remove(route);
			}
		}

		DateTime lastSaveTimeStamp;
		public bool ShouldSave => DateTime.UtcNow - lastSaveTimeStamp > TimeSpan.FromMinutes(SubscriptionPersistentIntervalInMinutes);
		public void Save(ILogWriter writer, AtomicCounter<ulong> counter) {
			foreach(var item in Subscriptions) {
				foreach(var subscriber in item.Subscribers) {
					var request = new SubscriptionRequest(subscriber, counter.NextId(), true, item.Pattern);
					writer.WriteLogEntry(new LogEntry(EntryType.In, request));
				}
			}
			this.lastSaveTimeStamp = DateTime.UtcNow;
		}

		public void UnsubscribeAll(string route) {
			foreach(var sub in subscriptions.ToArray()) {
				sub.Subscribers.Remove(route);
				if (!sub.Subscribers.Any()) {
					subscriptions.Remove(sub);
				}
			}
		}
	}
}