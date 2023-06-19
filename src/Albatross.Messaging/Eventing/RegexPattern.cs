using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Albatross.Messaging.Eventing {
	public record class RegexPattern {
		public string Pattern { get; init; }
		public Regex Regex { get; init; }
		public RegexPattern(string pattern) {
			Pattern = pattern;
			Regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
		}
		public ISet<string> Subscribers { get; init; } = new HashSet<string>();
		public bool Match(string topic) => Regex.IsMatch(topic);

		public override int GetHashCode() => Pattern.GetHashCode();
	}
}
