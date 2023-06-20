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
		public SubscriberState State { get; set; }
		public DateTime LastHeartbeat { get; set; }
		public override int GetHashCode() => Route.GetHashCode();
		public bool Equals(Subscriber? other) => Route.Equals(other?.Route);
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
		public ISet<Subscription> Subscriptions { get; init; } = new HashSet<Subscription>();

		public void Add(string pattern, string route) {
			var item = Subscriptions.GetOrAdd(item => item.Pattern == pattern, () => new Subscription(pattern));
			item.Subscribers.Add(new Subscriber(route));
		}
		public bool Remove(string pattern, Subscriber subscriber) {
			var item = Subscriptions.Where(args => args.Pattern == pattern).FirstOrDefault();
			if (item != null) {
				if (item.Subscribers.TryGetAndRemove(args => args.Equals(subscriber), out var _)) {
					return true;
				}
			}
			return false;
		}
	}
}