﻿using Albatross.Text;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Caching {
	public class KeyBuilder {
		List<object> parts = new List<object>();

		public KeyBuilder() { }

		public KeyBuilder(ICacheManagement cacheManagement, object key) {
			Add(cacheManagement, key);
		}
		public KeyBuilder AddKeys(params object[] items) {
			foreach (var item in items) { parts.Add(item); }
			return this;
		}
		public KeyBuilder Add<T>(ICacheManagement cacheManagement, T key) where T : notnull {
			parts.Add(cacheManagement.KeyPrefix);
			parts.Add(key);
			return this;
		}
		public KeyBuilder AddCache(ICacheManagement cacheManagement) {
			parts.Add(cacheManagement.KeyPrefix);
			return this;
		}
		public KeyBuilder AddKey<T>(T key) where T : notnull {
			parts.Add(key);
			return this;
		}
		public string BuildCacheResetKeyPattern(ICacheManagement cacheKeyManagement) {
			Add(cacheKeyManagement, string.Empty);
			return Build(true);
		}
		public string Build(bool postfixWildCard) {
			var sb = new StringBuilder();
			foreach (var item in parts) {
				sb.Append(item);
				if (!sb.EndsWith(ICacheKeyManagement.CacheKeyDelimiter)) {
					sb.Append(ICacheKeyManagement.CacheKeyDelimiter);
				}
			}
			if (postfixWildCard) {
				sb.Append(ICacheKeyManagement.Asterisk);
			}
			return sb.ToString().ToLower();
		}
		public override string ToString() => this.Build(false);
	}
}