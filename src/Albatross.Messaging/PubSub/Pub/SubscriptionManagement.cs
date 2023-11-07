using Albatross.Collections;
using System.Collections.Generic;
using System.Linq;
using Albatross.Messaging.Configurations;
using System.Text.Json;
using System.IO;

namespace Albatross.Messaging.PubSub.Pub {
	public interface ISubscriptionManagement {
		IEnumerable<Subscription> Subscriptions { get; }
		void Add(string pattern, string route);
		void Remove(string pattern, string route);
		void UnsubscribeAll(string route);
	}
	public class SubscriptionManagement : ISubscriptionManagement {
		private readonly SubscriptionManagementConfiguration config;
		public SubscriptionManagement(SubscriptionManagementConfiguration config) {
			this.config = config;
			Load();
		}

		ISet<Subscription> subscriptions = new HashSet<Subscription>();
		public IEnumerable<Subscription> Subscriptions => this.subscriptions;

		public void Add(string pattern, string route) {
			var subscription = subscriptions.GetOrAdd(item => item.Pattern == pattern, () => new Subscription(pattern));
			subscription.Subscribers.Add(route);
			Save();
		}

		public void Remove(string pattern, string route) {
			var item = subscriptions.Where(args => args.Pattern == pattern).FirstOrDefault();
			if (item != null) {
				item.Subscribers.Remove(route);
				Save();
			}
		}
		
		public void UnsubscribeAll(string route) {
			foreach(var sub in subscriptions.ToArray()) {
				sub.Subscribers.Remove(route);
				if (!sub.Subscribers.Any()) {
					subscriptions.Remove(sub);
				}
			}
			Save();
		}

		public void Save() {
			using (var stream = File.OpenWrite(this.config.DiskStorage.FileName)) {
				JsonSerializer.Serialize<IEnumerable<Subscription>>(subscriptions);
			}
		}
		public void Load() {
			if (File.Exists(this.config.DiskStorage.FileName)) {
				using (var stream = File.OpenRead(this.config.DiskStorage.FileName)) {
					var items = JsonSerializer.Deserialize<IEnumerable<Subscription>>(stream) 
						?? new List<Subscription>();
					this.subscriptions = new HashSet<Subscription>(items);
				}
			} else {
				this.subscriptions.Clear();
			}
		}
	}
}