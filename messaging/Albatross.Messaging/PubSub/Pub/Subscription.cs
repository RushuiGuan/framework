using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Albatross.Messaging.PubSub.Pub {
	public class Subscription {
		public string Pattern { get; init; }
		[JsonIgnore]
		public Regex Regex { get; init; }
		public Subscription(string pattern) {
			Pattern = pattern;
			Regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
		}
		public bool Match(string topic) => Regex.IsMatch(topic);
		public override int GetHashCode() => Pattern.GetHashCode();
		public ISet<string> Subscribers { get; init; } = new HashSet<string>();
	}
}