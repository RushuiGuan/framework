using Albatross.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Albatross.Messaging.Eventing.Pub {
	public enum SubscriberState {
		Connected, Lost,
	}
	public sealed record class Subscriber : IEquatable<Subscriber> {
		public string Route { get; init; }
		public Subscriber(string route) {
			Route = route;
			LastHeartbeat = DateTime.Now;
			State = SubscriberState.Connected;
		}
		public SubscriberState State { get; private set; }
		public DateTime LastHeartbeat { get; private set; }
		public override int GetHashCode() => Route.GetHashCode();
		public bool Equals(Subscriber? other) => Route.Equals(other?.Route);

		public Subscriber SetState(SubscriberState state) {
			this.State = state;
			return this;
		}
		public Subscriber SetHeartBeat() {
			this.LastHeartbeat = DateTime.Now;
			return this;
		}

		public void CheckState(TimeSpan threshold) {
			if(DateTime.Now - LastHeartbeat > threshold) {
				this.State = SubscriberState.Lost;
			}
		}
	}
	public class Subscription {
		public string Pattern { get; init; }
		public Regex Regex { get; init; }
		public Subscription(string pattern) {
			Pattern = pattern;
			Regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
		}
		public bool Match(string topic) => Regex.IsMatch(topic);
		public override int GetHashCode() => Pattern.GetHashCode();
		public ISet<Subscriber> Subscribers { get; init; } = new HashSet<Subscriber>();
	}

	public class SubscriptionManagement {
		ISet<Subscription> subscriptions = new HashSet<Subscription>();
		ISet<Subscriber> subscribers = new HashSet<Subscriber>();

		public IEnumerable<Subscriber> Subscribers => this.subscribers;
		public IEnumerable<Subscription> Subscriptions => this.subscriptions;

		public Subscriber Add(string pattern, string route) {
			var subscription = subscriptions.GetOrAdd(item => item.Pattern == pattern, () => new Subscription(pattern));
			var subscriber = subscription.Subscribers.GetOrAdd(args=> args.Route == route, ()=> { 
				var sub = new Subscriber(route);
				subscribers.Add(sub);
				return sub;
			});
			return subscriber;
		}
		public bool Remove(string pattern, string route) {
			var item = subscriptions.Where(args => args.Pattern == pattern).FirstOrDefault();
			if (item != null) {
				if (item.Subscribers.TryGetAndRemove(args => args.Route == route, out var sub)) {
					subscribers.Remove(sub);
					return true;
				}
			}
			return false;
		}
	}
}