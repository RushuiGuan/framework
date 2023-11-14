using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public class CompositeKeyBuilder {
		List<object> parts = new List<object>();

		public CompositeKeyBuilder(ICacheManagement cacheManagement) {
			Add(cacheManagement.KeyPrefix);
		}

		public CompositeKeyBuilder(params object[] compositeKeys) {
			Add(compositeKeys);
		}

		public CompositeKeyBuilder Add(params object[] composite) {
			parts.AddRange(composite);
			return this;
		}
		public string Build(bool postfixWildCard) {
			var sb = new StringBuilder();
			foreach (var item in parts) {
				sb.Append(item);
				if (sb[sb.Length - 1] != ICacheKeyManagement.CacheKeyDelimiter) {
					sb.Append(ICacheKeyManagement.CacheKeyDelimiter);
				}
			}
			if(postfixWildCard) {
				sb.Append(ICacheKeyManagement.Asterisk);
			}
			return sb.ToString().ToLowerInvariant();
		}
		public override string ToString() => this.Build(false);
	}
}